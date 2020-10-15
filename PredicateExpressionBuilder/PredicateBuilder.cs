using System;
using System.Linq.Expressions;

namespace PredicateExpressionBuilder
{
    public class PredicateBuilder<T>
    {
        private readonly bool _fallback;
        private Expression<Func<T, bool>> _predicate;

        public PredicateBuilder(bool fallback)
        {
            _fallback = fallback;
            _predicate = null;
        }
        
        public PredicateBuilder(Expression<Func<T, bool>> expression)
        {
            _predicate = expression ?? throw new ArgumentNullException(nameof(expression));
        }
        
        public PredicateBuilder<T> And(Expression<Func<T, bool>> expression)
        {
            _predicate = _predicate == null ? expression : _predicate.And(expression);
            return this;
        }
        
        public PredicateBuilder<T> Or(Expression<Func<T, bool>> expression)
        {
            _predicate = _predicate == null ? expression : _predicate.Or(expression);
            return this;
        }

        public Expression<Func<T, bool>> Build()
        {
            return _predicate ?? (_ => _fallback);
        }
        
        public static implicit operator Expression<Func<T, bool>>(PredicateBuilder<T> builder)
        {
            return builder.Build();
        }
    }
}