using System;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Utilities
{
    public static class ExpressionUtils
    {
        public static Expression<Func<T, bool>> BuildPredicate<T>(string propertyName, string comparison, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var left = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
            var body = MakeComparison(left, comparison, value);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildPredicate<T>(Expression<Func<T, bool>> leftExpression, string propertyName, string comparison, string value, string operand)
        {
            var parameter = leftExpression.Parameters[0];
            var left = propertyName.Split('.').Aggregate((Expression)leftExpression.Parameters[0], Expression.Property);
            var body = MakeComparison(left, comparison, value);
            var right = Expression.Lambda<Func<T, bool>>(body, parameter);

            var binaryExpression = Expression.MakeBinary(GetExpressionType(operand), leftExpression.Body, right.Body);

            return Expression.Lambda<Func<T, bool>>(binaryExpression, parameter);
        }

        public static Expression<Func<T, bool>> BuildPredicate<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression, string operand)
        {
            var parameter = leftExpression.Parameters[0];
            var right = Expression.Lambda<Func<T, bool>>(rightExpression.Body, parameter);

            var binaryExpression = Expression.MakeBinary(GetExpressionType(operand), leftExpression.Body, right.Body);

            return Expression.Lambda<Func<T, bool>>(binaryExpression, parameter);
        }

        public static Expression<Func<T, bool>> BuildPredicate<T>(Expression<Func<T, bool>> expression, string operand)
        {
            var parameter = expression.Parameters[0];
            UnaryExpression unaryExpression = null;

            switch (operand)
            {
                case "not":
                    unaryExpression = Expression.Not(expression.Body);
                    break;
            }

            if (unaryExpression == null)
            {
                return expression;
            }
            return Expression.Lambda<Func<T, bool>>(unaryExpression, parameter);
        }



        private static ExpressionType GetExpressionType(string operand)
        {
            switch (operand)
            {
                case "or":
                    return ExpressionType.Or;
                case "and":
                    return ExpressionType.And;
                default:
                    return ExpressionType.Or;
            }
        }


        private static Expression MakeComparison(Expression left, string comparison, string value)
        {
            switch (comparison)
            {
                case "equal":
                    return MakeBinary(ExpressionType.Equal, left, value);
                case "notEqual":
                    return MakeBinary(ExpressionType.NotEqual, left, value);
                case "greaterThan":
                    return MakeBinary(ExpressionType.GreaterThan, left, value);
                case "greaterThanOrEqual":
                    return MakeBinary(ExpressionType.GreaterThanOrEqual, left, value);
                case "lessThan":
                    return MakeBinary(ExpressionType.LessThan, left, value);
                case "lessThanOrEqual":
                    return MakeBinary(ExpressionType.LessThanOrEqual, left, value);
                case "contains":
                case "startsWith":
                case "endsWith":
                    return Expression.Call(MakeString(left), comparison, Type.EmptyTypes, Expression.Constant(value, typeof(string)));
                default:
                    throw new NotSupportedException($"Invalid comparison operator '{comparison}'.");
            }
        }

        private static Expression MakeString(Expression source)
        {

            return source.Type == typeof(string) ? source : Expression.Call(source, "ToString", Type.EmptyTypes);
        }

        private static Expression MakeBinary(ExpressionType type, Expression left, string value)
        {
            object typedValue = value;
            if (left.Type != typeof(string))
            {
                if (string.IsNullOrEmpty(value))
                {
                    typedValue = null;
                    if (Nullable.GetUnderlyingType(left.Type) == null)
                        left = Expression.Convert(left, typeof(Nullable<>).MakeGenericType(left.Type));
                }
                else
                {
                    var valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
                    typedValue = valueType.IsEnum ? Enum.Parse(valueType, value) :
                        valueType == typeof(Guid) ? Guid.Parse(value) :
                        Convert.ChangeType(value, valueType);
                }
            }
            var right = Expression.Constant(typedValue, left.Type);
            return Expression.MakeBinary(type, left, right);
        }
    }
}