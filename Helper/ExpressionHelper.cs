using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CTS_BE.Helper
{
    public static class ExpressionHelper
    {
        public static Expression<Func<T, bool>> GetFilterExpression<T>(string field, dynamic value, string op)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression propertyAccess;

            if (field.Contains('.'))
            {
                propertyAccess = GetNestedPropertyAccess(parameter, field);
            }
            else
            {
                propertyAccess = GetPropertyAccess<T>(parameter, field);
            }

            var convertedValue = Expression.Constant(Convert.ChangeType(value, propertyAccess.Type));

            Expression predicate;

            switch (op.ToLower())
            {
                case "equals":
                    predicate = Expression.Equal(propertyAccess, convertedValue);
                    break;
                case "greaterthan":
                    predicate = Expression.GreaterThan(propertyAccess, convertedValue);
                    break;
                case "lessthan":
                    predicate = Expression.LessThan(propertyAccess, convertedValue);
                    break;
                case "startswith":
                    predicate = Expression.Call(propertyAccess, "StartsWith", null, convertedValue);
                    break;
                case "contains":
                    predicate = Expression.Call(propertyAccess, "Contains", null, convertedValue);
                    break;
                case "notcontains":
                    predicate = Expression.Not(Expression.Call(propertyAccess, "Contains", null, convertedValue));
                    break;
                case "endswith":
                    predicate = Expression.Call(propertyAccess, "EndsWith", null, convertedValue);
                    break;
                case "notequal":
                    predicate = Expression.NotEqual(propertyAccess, convertedValue);
                    break;
                default:
                    throw new ArgumentException("Invalid operator.");
            }

            return Expression.Lambda<Func<T, bool>>(predicate, parameter);
        }
        private static Expression GetNestedPropertyAccess(ParameterExpression parameter, string field)
        {
            string[] fieldParts = field.Split('.');
            Expression propertyAccess = parameter;

            foreach (string propertyName in fieldParts)
            {
                PropertyInfo property = propertyAccess.Type.GetProperty(propertyName);
                if (property == null)
                {
                    throw new ArgumentException($"Property '{propertyName}' not found on type '{propertyAccess.Type}'.");
                }

                propertyAccess = Expression.Property(propertyAccess, property);
            }

            return propertyAccess;
        }

        private static Expression GetPropertyAccess<T>(ParameterExpression parameter, string field)
        {
            PropertyInfo property = typeof(T).GetProperty(field);
            if (property == null)
            {
                throw new ArgumentException($"Property '{field}' not found on type '{typeof(T)}'.");
            }

            return Expression.Property(parameter, property);
        }
    }
}
