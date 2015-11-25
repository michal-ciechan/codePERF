using System.Collections.Generic;

namespace TddMockThrowInterfaceCallVerificationTechnique.SUT
{
    public interface IRepository
    {
        List<int> GetData();
        void SaveData();
    }
}