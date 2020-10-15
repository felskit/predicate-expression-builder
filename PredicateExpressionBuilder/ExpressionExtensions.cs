using System;
using System.Linq.Expressions;

namespace PredicateExpressionBuilder
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            var rightBody = right.Body.Replace(right.Parameters[0], left.Parameters[0]);
            var body = Expression.AndAlso(left.Body, rightBody);
            return Expression.Lambda<Func<T, bool>>(body, left.Parameters);
        }
        
        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            var rightBody = right.Body.Replace(right.Parameters[0], left.Parameters[0]);
            var body = Expression.OrElse(left.Body, rightBody);
            return Expression.Lambda<Func<T, bool>>(body, left.Parameters);
        }
        
        public static Expression Replace(this Expression expression, Expression oldExpression, Expression newExpression)
        {
            return new ReplaceExpressionVisitor(oldExpression, newExpression).Visit(expression);
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldExpression;
            private readonly Expression _newExpression;

            public ReplaceExpressionVisitor(Expression oldExpression, Expression newExpression)
            {
                _oldExpression = oldExpression;
                _newExpression = newExpression;
            }
            
            public override Expression Visit(Expression node)
            {
                return node == _oldExpression ? _newExpression : base.Visit(node);
            }
        }
    }
}