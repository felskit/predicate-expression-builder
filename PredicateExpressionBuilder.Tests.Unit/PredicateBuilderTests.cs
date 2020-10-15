using System;
using System.Linq.Expressions;
using FluentAssertions;
using Xunit;

namespace PredicateExpressionBuilder.Tests.Unit
{
    public class UnitTest1
    {
        [Fact]
        public void ShouldConvertToExpressionImplicitly()
        {
            Expression<Func<object, bool>> _ = new PredicateBuilder<object>(true);
        }
        
        [Fact]
        public void ShouldBuildAndConvertToTheSameExpression()
        {
            var builder = new PredicateBuilder<object>(_ => true);
            Expression<Func<object, bool>> expression1 = builder;
            Expression<Func<object, bool>> expression2 = builder.Build();
            expression1.Should().Be(expression2);
        }
        
        [Fact]
        public void ShouldThrow_ArgumentNullException_WhenInitializedWithNullExpression()
        {
            Expression<Func<object, bool>> nullExpression = null;
            Action action = () => new PredicateBuilder<object>(nullExpression);
            action.Should().Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void ShouldUseDefaultFallback_True()
        {
            PredicateBuilder<object> builder = new PredicateBuilder<object>(true);
            Expression<Func<object, bool>> expression = builder.Build();
            bool result = expression.Compile().Invoke(new object());
            result.Should().BeTrue();
        }
        
        [Fact]
        public void ShouldUseDefaultFallback_False()
        {
            PredicateBuilder<object> builder = new PredicateBuilder<object>(false);
            Expression<Func<object, bool>> expression = builder.Build();
            bool result = expression.Compile().Invoke(new object());
            result.Should().BeFalse();
        }
        
        [Fact]
        public void ShouldNotUseDefaultFallback_True_And()
        {
            PredicateBuilder<object> builder = new PredicateBuilder<object>(false).And(_ => true);
            Expression<Func<object, bool>> expression = builder.Build();
            bool result = expression.Compile().Invoke(new object());
            result.Should().BeTrue();
        }
        
        [Fact]
        public void ShouldNotUseDefaultFallback_False_And()
        {
            PredicateBuilder<object> builder = new PredicateBuilder<object>(true).And(_ => false);
            Expression<Func<object, bool>> expression = builder.Build();
            bool result = expression.Compile().Invoke(new object());
            result.Should().BeFalse();
        }
        
        [Fact]
        public void ShouldNotUseDefaultFallback_True_Or()
        {
            PredicateBuilder<object> builder = new PredicateBuilder<object>(false).Or(_ => true);
            Expression<Func<object, bool>> expression = builder.Build();
            bool result = expression.Compile().Invoke(new object());
            result.Should().BeTrue();
        }
        
        [Fact]
        public void ShouldNotUseDefaultFallback_False_Or()
        {
            PredicateBuilder<object> builder = new PredicateBuilder<object>(true).Or(_ => false);
            Expression<Func<object, bool>> expression = builder.Build();
            bool result = expression.Compile().Invoke(new object());
            result.Should().BeFalse();
        }
        
        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        public void ShouldCombineExpressionsCorrectly_And(bool left, bool right, bool expected)
        {
            PredicateBuilder<object> builder = new PredicateBuilder<object>(_ => left).And(_ => right);
            Expression<Func<object, bool>> expression = builder.Build();
            bool result = expression.Compile().Invoke(new object());
            result.Should().Be(expected);
        }
        
        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        [InlineData(false, false, false)]
        public void ShouldCombineExpressionsCorrectly_Or(bool left, bool right, bool expected)
        {
            PredicateBuilder<object> builder = new PredicateBuilder<object>(_ => left).Or(_ => right);
            Expression<Func<object, bool>> expression = builder.Build();
            bool result = expression.Compile().Invoke(new object());
            result.Should().Be(expected);
        }
    }
}