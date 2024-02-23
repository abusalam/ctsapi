using System;
using System.Linq.Expressions;

namespace CTS_BE.Helper
{
    public static class ExpressionHelper
    {
        public static Expression<Func<T, bool>> GetFilterExpression<T>(string field, string value, string op)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, field);
            var convertedValue = Expression.Constant(Convert.ChangeType(value, property.Type));

            Expression predicate;

            switch (op.ToLower())
            {
                case "equal":
                    predicate = Expression.Equal(property, convertedValue);
                    break;
                case "greaterthan":
                    predicate = Expression.GreaterThan(property, convertedValue);
                    break;
                case "lessthan":
                    predicate = Expression.LessThan(property, convertedValue);
                    break;
                default:
                    throw new ArgumentException("Invalid operator.");
            }

            return Expression.Lambda<Func<T, bool>>(predicate, parameter);
        }
    }
}
