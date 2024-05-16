using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace PES.Infrastructure.Common
{
    public static class DynamicRelationalExtensions
    {
        static MethodInfo UpdateMethodInfo =
            typeof(RelationalQueryableExtensions).GetMethod(nameof(RelationalQueryableExtensions.ExecuteUpdate));

        static MethodInfo UpdateAsyncMethodInfo =
            typeof(RelationalQueryableExtensions).GetMethod(nameof(RelationalQueryableExtensions.ExecuteUpdateAsync));

        public static int ExecuteUpdate(this IQueryable query, string fieldName, object? fieldValue)
        {
            var updateBody = BuildUpdateBody(query.ElementType,
                new Dictionary<string, object?> { { fieldName, fieldValue } });

            return (int)UpdateMethodInfo.MakeGenericMethod(query.ElementType).Invoke(null, new object?[] { query, updateBody });
        }

        public static Task<int> ExecuteUpdateAsync(this IQueryable query, string fieldName, object? fieldValue, CancellationToken cancellationToken = default)
        {
            var updateBody = BuildUpdateBody(query.ElementType,
                new Dictionary<string, object?> { { fieldName, fieldValue } });

            return (Task<int>)UpdateAsyncMethodInfo.MakeGenericMethod(query.ElementType).Invoke(null, new object?[] { query, updateBody, cancellationToken })!;
        }

        public static int ExecuteUpdate(this IQueryable query, IReadOnlyDictionary<string, object?> fieldValues)
        {
            var updateBody = BuildUpdateBody(query.ElementType, fieldValues);

            return (int)UpdateMethodInfo.MakeGenericMethod(query.ElementType).Invoke(null, new object?[] { query, updateBody });
        }

        public static Task<int> ExecuteUpdateAsync(this IQueryable query, IReadOnlyDictionary<string, object?> fieldValues, CancellationToken cancellationToken = default)
        {
            var updateBody = BuildUpdateBody(query.ElementType, fieldValues);

            return (Task<int>)UpdateAsyncMethodInfo.MakeGenericMethod(query.ElementType).Invoke(null, new object?[] { query, updateBody, cancellationToken })!;
        }

        static LambdaExpression BuildUpdateBody(Type entityType, IReadOnlyDictionary<string, object?> fieldValues)
        {
            var setParam = Expression.Parameter(typeof(SetPropertyCalls<>).MakeGenericType(entityType), "s");
            var objParam = Expression.Parameter(entityType, "e");

            Expression setBody = setParam;

            foreach (var pair in fieldValues)
            {
                var propExpression = Expression.PropertyOrField(objParam, pair.Key);
                var valueExpression = ValueForType(propExpression.Type, pair.Value);

                // s.SetProperty(e => e.SomeField, value)
                setBody = Expression.Call(setBody, nameof(SetPropertyCalls<object>.SetProperty),
                    new[] { propExpression.Type }, Expression.Lambda(propExpression, objParam), valueExpression);

            }

            // s => s.SetProperty(e => e.SomeField, value)
            var updateBody = Expression.Lambda(setBody, setParam);

            return updateBody;
        }

        static Expression ValueForType(Type desiredType, object? value)
        {
            if (value == null)
            {
                return Expression.Default(desiredType);
            }

            if (value.GetType() != desiredType)
            {
                return Expression.Convert(Expression.Constant(value), desiredType);
            }

            return Expression.Constant(value);
        }
    }
}