using System;
using System.Collections.Generic;

namespace TddMockThrowInterfaceCallVerificationTechnique.SUT
{
    public class SomeController
    {
        private readonly IRepository _repo;
        private readonly IWorker _worker;

        public SomeController(IRepository repo, IWorker worker)
        {
            _repo = repo;
            _worker = worker;
        }

        public virtual List<int> ProcessData(List<int> data)
        {
            var newData = _worker.DoWork(data);

            if (newData.Count == 0)
                return data; // Do Something else

            SaveData(newData);

            return newData;
        }

        internal void Iteration()
        {
            var data = _repo.GetData();

            if (data.Count == 0) return;

            var processedData = ProcessData(data);

            if (processedData == data) return;

            Console.WriteLine("Different Data");
        }

        internal void SaveData(List<int> newData)
        {
            throw new NotImplementedException();
        }
    }
}