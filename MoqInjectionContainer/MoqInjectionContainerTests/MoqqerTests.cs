using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Moq;
using MoqInjectionContainer;
using MoqInjectionContainerTests.Helpers;
using NUnit.Framework;

namespace MoqInjectionContainerTests
{
    [TestFixture]
    public class MoqqerTests
    {
        private MockRepository _factory;
        private Moqqer _moq;
        private SomeClass _subject;

        [SetUp]
        public void A_Setup()
        {
            _moq = new Moqqer();

            _factory = new MockRepository(MockBehavior.Default) {DefaultValue = DefaultValue.Mock};

            _subject = _moq.Get<SomeClass>();
        }


        [Test]
        public void CallA_CallsDependencyA()
        {
            _subject.CallA();

            _moq.Of<IDepencyA>().Verify(x => x.Call(), Times.Once);
        }

        [Test]
        public void Mock_Call_ParameterlessMethod_ReturningInterface_ReturnsMockedInterface()
        {
            var res = _subject.Mock.GetA();

            res.Should().NotBeNull();
        }
        
        [Test]
        public void Type_GetMethods_ReturnsAllPublic()
        {
            var type = typeof (SomeClass);

            var res = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var methodInfo in res)
            {
                Console.WriteLine(methodInfo.Name);
            }
        }

        [Test]
        public void GetMockableMethods_SomeClass_ReturnsAllPublic()
        {
            var type = typeof (IMockSetup);

            var methods = Moqqer.GetMockableMethods(type).Select(x => x.Name).ToList();

            methods.Should().BeEquivalentTo(new [] {"GetA", "GetB"});
        }

        [Test]
        public void SetupMockMethods_SomeClassGetA_ShouldNotBeNull()
        {
            var mock = new Mock<IMockSetup>();

            var type = typeof (IMockSetup);

            _moq.SetupMockMethods(mock, type);

            mock.Object.GetA().Should().NotBeNull();
        }

        [Test]
        public void MockOf_IInterfaceWithGenericMethod_CanSetup()
        {
            var res = _moq.Of<IInterfaceWithGenericMethod>();
        }


        [Test]
        public void GetMockableMethods_IInterfaceWithGenericMethod_DoesNotReturnGenericMethod()
        {
            var type = typeof(IInterfaceWithGenericMethod);

            var methods = Moqqer.GetMockableMethods(type).Select(x => x.Name).ToList();

            methods.Should().BeEmpty();
        }
    }
}