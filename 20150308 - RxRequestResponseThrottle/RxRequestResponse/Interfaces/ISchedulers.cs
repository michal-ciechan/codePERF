using System.Reactive.Concurrency;

namespace RxRequestResponse.Interfaces
{
    public interface ISchedulers
    {
        IScheduler CurrentThread { get; }
        IScheduler Dispatcher { get; }
        IScheduler NewThread { get; }
        IScheduler Immediate { get; }
        IScheduler ThreadPool { get; }
    }
}