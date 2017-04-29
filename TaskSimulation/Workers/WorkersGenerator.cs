using System.Collections.Generic;
using TaskSimulation.Distribution;

namespace TaskSimulation.Workers
{
    public class WorkersGenerator : IWorkersGenerator
    {
        private const int INITIAL_NUM_OF_WORKERS = 20;

        public List<Worker> WokersPool { get; private set; }
        private int _position = 0;

        public WorkersGenerator()
        {
            WokersPool = new List<Worker>();

            AddWorkers(INITIAL_NUM_OF_WORKERS);
        }

        public void AddWorkers(int numOfWorkers)
        {
            for (int i = 0; i < numOfWorkers; i++)
            {
                WokersPool.Add(new Worker());
            }
        }

        public Worker GetNextWorker()
        {
            if (_position >= WokersPool.Count)
                return null;

            return WokersPool[_position++];
        }
    }
}
