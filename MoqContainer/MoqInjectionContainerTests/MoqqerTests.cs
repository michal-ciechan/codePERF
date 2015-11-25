using Moq;
using MoqInjectionContainer;
using MoqInjectionContainerTests.Helpers;
using NUnit.Framework;

namespace MoqInjectionContainerTests
{
    [TestFixture]
    public class MoqqerTests
    {
        private Moqqer _moq;
        private SomeClass _subject;
        private MockRepository _factory;

        [SetUp]
        public void Setup()
        {
            _moq = new Moqqer();


            _factory = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };





            _subject = _moq.Get<SomeClass>();
        }



        [Test]
        public void MockRepo_CallA_CallsDependencyA()
        {
            _subject.CallA();

            _factory.Create<IDepencyA>().Verify(x => x.Call(),Times.Once);
        }
    

    [Test]
        public void CallA_CallsDependencyA()
        {

            _subject.CallA();


            _moq.Of<IDepencyA>().Verify(x => x.Call(), Times.Once);
        }
    }
}
