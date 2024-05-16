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

    private static Expression GetPropertyExpression(PropertyInfo prop, ParameterExpression paramExpr, ConstantExpression valueExpr)
    {

        var memberAcc = Expression.MakeMemberAccess(paramExpr, prop);
        var containsMember = typeof(string).GetMethods().Where(x => x.Name == "Contains").FirstOrDefault();
        return Expression.Call(memberAcc, containsMember!, valueExpr);
    }
}
}