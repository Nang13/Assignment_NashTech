using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace PES.Application.Utilities
{
    public static class FilterUtilities
    {
        public static IEnumerable<T> SelectItems<T>(IEnumerable<T> items, string propName, string value)
        {
            IEnumerable<PropertyInfo> props;
            if (!string.IsNullOrEmpty(propName))
                props = new PropertyInfo[] { typeof(T).GetProperty(propName)! };
            else
                props = typeof(T).GetProperties();
            props = props.Where(x => x != null && x.PropertyType == typeof(string));
            Expression lastExpr = null!;
            ParameterExpression paramExpr = Expression.Parameter(typeof(T), "x");
            ConstantExpression valueExpr = Expression.Constant(value);
            foreach (var prop in props)
            {
                var propExpr = GetPropertyExpression(prop, paramExpr, valueExpr);
                if (lastExpr == null)
                    lastExpr = propExpr;
                else
                    lastExpr = Expression.MakeBinary(ExpressionType.Or, lastExpr, propExpr);
            }
            if (lastExpr == null)
                return new T[] { };
            var filterExpr = Expression.Lambda(lastExpr, paramExpr);
            return items.Where<T>((Func<T, bool>)filterExpr.Compile());
        }

        public static IEnumerable<T> SelectId<T>(IEnumerable<T> items, string propName, string value)
        {
            // Try to parse the value into a Guid
            if (!Guid.TryParse(value, out Guid guidValue))
            {
                throw new ArgumentException("The provided value is not a valid Guid.", nameof(value));
            }

            // Get the property info for the specified property name
            var prop = typeof(T).GetProperty(propName);
            if (prop == null || prop.PropertyType != typeof(Guid))
            {
                throw new ArgumentException($"The property '{propName}' does not exist or is not of type Guid.", nameof(propName));
            }

            // Build the lambda expression
            var paramExpr = Expression.Parameter(typeof(T), "x");
            var propExpr = Expression.Property(paramExpr, prop);
            var valueExpr = Expression.Constant(guidValue, typeof(Guid));
            var equalExpr = Expression.Equal(propExpr, valueExpr);
            var filterExpr = Expression.Lambda<Func<T, bool>>(equalExpr, paramExpr);

            // Apply the filter to the items
            return items.Where<T>((Func<T, bool>)filterExpr.Compile());
        }

        private static Expression GetPropertyExpression(PropertyInfo prop, ParameterExpression paramExpr, ConstantExpression valueExpr)
        {
            var memberAcc = Expression.MakeMemberAccess(paramExpr, prop);
            var containsMember = typeof(string).GetMethods().Where(x => x.Name == "Contains").FirstOrDefault();
            return Expression.Call(memberAcc, containsMember!, valueExpr);
        }
    }
}