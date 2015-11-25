using System.Reactive.Concurrency;
using RxRequestResponse.Interfaces;

namespace RxRequestResponseTests.Helpers
{
    public sealed class TestSchedulers : ISchedulers
    {
        private readonly TestSchedulerEx _currentThread = new TestSchedulerEx("CurrentThread");
        private readonly TestSchedulerEx _dispatcher = new TestSchedulerEx("Dispatcher");
        private readonly TestSchedulerEx _immediate = new TestSchedulerEx("Immediate");
        private readonly TestSchedulerEx _newThread = new TestSchedulerEx("NewThread");
        private readonly TestSchedulerEx _threadPool = new TestSchedulerEx("ThreadPool");
        #region Explicit implementation of ISchedulerService
        IScheduler ISchedulers.CurrentThread { get { return _currentThread; } }
        IScheduler ISchedulers.Dispatcher { get { return _dispatcher; } }
        IScheduler ISchedulers.Immediate { get { return _immediate; } }
        IScheduler ISchedulers.NewThread { get { return _newThread; } }
        IScheduler ISchedulers.ThreadPool { get { return _threadPool; } }
        #endregion
        public TestSchedulerEx CurrentThread { get { return _currentThread; } }
        public TestSchedulerEx Dispatcher { get { return _dispatcher; } }
        public TestSchedulerEx Immediate { get { return _immediate; } }
        public TestSchedulerEx NewThread { get { return _newThread; } }
        public TestSchedulerEx ThreadPool { get { return _threadPool; } }
    }
}