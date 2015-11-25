using System.Collections.Generic;

namespace TddMockThrowInterfaceCallVerificationTechnique.SUT
{
    public interface IWorker
    {
        List<int> DoWork(List<int> data);
    }
}