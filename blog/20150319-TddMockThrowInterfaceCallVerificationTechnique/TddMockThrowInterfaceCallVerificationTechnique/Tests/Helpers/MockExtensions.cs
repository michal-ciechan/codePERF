using System;
using System.Linq.Expressions;
using Moq;
using Moq.Language.Flow;

namespace TddMockThrowInterfaceCallVerificationTechnique.Tests.Helpers
{
    public static class MockExtensions
    {
        public static void Throws<TMock, TResult>(this ISetup<TMock, TResult> mock)
            where TMock : class
        {
            mock.Callback(() => { throw new MockCalled(); });
        }

        public static void ThrowsOn<T, TResult>(this Mock<T> mock, Expression<Func<T, TResult>> expression)
            where T : class
        {
            mock.Setup(expression).Throws();
        }
    }
}