using System;
using System.Linq.Expressions;

namespace CTS_BE.Helper
{
    public static class ExpressionHelper
    {
        public static Expression<Func<T, bool>> GetFilterExpression<T>(string field, dynamic value, string op)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, field);
            var convertedValue = Expression.Constant(Convert.ChangeType(value, property.Type));

            Expression predicate;

            switch (op.ToLower())
            {
                case "equals":
                    predicate = Expression.Equal(property, convertedValue);
                    break;
                case "greaterthan":
                    predicate = Expression.GreaterThan(property, convertedValue);
                    break;
                case "lessthan":
                    predicate = Expression.LessThan(property, convertedValue);
                    break;
                case "startswith":
                    predicate = Expression.Call(property, "StartsWith", null, convertedValue);
                    break;
                case "contains":
                    predicate = Expression.Call(property, "Contains", null, convertedValue);
                    break;
                case "notcontains":
                    predicate = Expression.Not(Expression.Call(property, "Contains", null, convertedValue));
                    break;
                case "endswith":
                    predicate = Expression.Call(property, "EndsWith", null, convertedValue);
                    break;
                case "notequal":
                    predicate = Expression.NotEqual(property, convertedValue);
                    break;
                default:
                    throw new ArgumentException("Invalid operator.");
            }

            return Expression.Lambda<Func<T, bool>>(predicate, parameter);
        }
    }
}
