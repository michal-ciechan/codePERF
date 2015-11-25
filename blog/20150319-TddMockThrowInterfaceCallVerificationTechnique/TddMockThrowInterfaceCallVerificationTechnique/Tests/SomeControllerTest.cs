using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TddMockThrowInterfaceCallVerificationTechnique.SUT;
using TddMockThrowInterfaceCallVerificationTechnique.Tests.Helpers;

namespace TddMockThrowInterfaceCallVerificationTechnique.Tests
{
    [TestFixture]
    public class SomeControllerTest
    {
        private SomeController _controller;
        private Mock<SomeController> _mock;
        private Mock<IRepository> _repo;
        private Mock<IWorker> _worker;

        [SetUp]
        public void Ao_Setup()
        {
            _repo = new Mock<IRepository>();
            _worker = new Mock<IWorker>();
            _controller = new SomeController(_repo.Object, _worker.Object);
            _mock = new Mock<SomeController>(_repo.Object, _worker.Object);
        }

        [Test]
        public void Iteration_HasData_CallsProcessData()
        {
            // Setup repo
            var list = new List<int> {1};
            _repo.Setup(r => r.GetData()).Returns(list);

            // Setup mock
            _mock.Setup(x => x.ProcessData(list))
                .Verifiable();

            // Act
            _mock.Object.Iteration();

            // Assert
            _mock.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof (MockCalled))]
        public void Iteration_HasData_CallsProcessDataThrow()
        {
            // Setup repo
            var list = new List<int> {1};
            _repo.Setup(r => r.GetData()).Returns(list);

            // Setup mock
            _mock.ThrowsOn(x => x.ProcessData(list));

            // Act
            _mock.Object.Iteration();
        }

        [Test]
        [ExpectedException(typeof (MockCalled))]
        public void Iteration_HasData_CallsProcessDataWhichCallsWorker()
        {
            // Setup repo
            var list = new List<int> {1};
            _repo.Setup(r => r.GetData()).Returns(list);

            // Setup Worker
            _worker.ThrowsOn(x => x.DoWork(list));

            // Act
            _controller.Iteration();
        }

        [Test]
        [ExpectedException(typeof (MockCalled))]
        public void Iteration_HasData_CallsWorker()
        {
            // Setup repo
            var list = new List<int> {1};
            _repo.Setup(r => r.GetData()).Returns(list);

            // Setup worker
            _worker.Setup(w => w.DoWork(list))
                .Throws(); // Technique

            _controller.Iteration();
        }

        [Test]
        [ExpectedException(typeof (MockCalled))]
        public void ProcessData_CallsWorker()
        {
            var list = new List<int> {1};

            // Setup worker
            _worker.ThrowsOn(w => w.DoWork(list));

            // Act
            _controller.ProcessData(list);
        }
    }
}