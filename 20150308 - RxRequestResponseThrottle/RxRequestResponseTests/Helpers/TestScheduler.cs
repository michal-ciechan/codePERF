using System;
using System.Reactive.Concurrency;

namespace RxRequestResponseTests.Helpers
{
    public class TestSchedulerEx : Microsoft.Reactive.Testing.TestScheduler
    {
        private DateTimeOffset _nextSchedule = new DateTimeOffset();
        public string Name { get; set; }

        public TestSchedulerEx(string name)
        {
            Name = name;
        }

        public new void AdvanceBy(long ticks)
        {
            Console.WriteLine("\r\n\r\nAdvancing {0} by {1}", Name, ticks);
            base.AdvanceBy(ticks);
        }

        public void ScheduleNext(Action action)
        {
            if (Now >= _nextSchedule) _nextSchedule = Now.AddTicks(1);

            this.Schedule(_nextSchedule, action);

            _nextSchedule = _nextSchedule.AddTicks(1);
        }

    }
}
