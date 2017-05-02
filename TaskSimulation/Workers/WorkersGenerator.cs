using System.Collections.Generic;

namespace TaskSimulation.Workers
{
    public class WorkersGenerator : IWorkersGenerator
    {
        private const int INITIAL_NUM_OF_WORKERS = 20;
        private long _workerID = 0;
        private readonly List<Worker> _wokersPool;
        private int _position = 0;

        public WorkersGenerator()
        {
            _wokersPool = new List<Worker>();

            AddWorkers(INITIAL_NUM_OF_WORKERS);
        }

        public void AddWorkers(int numOfWorkers)
        {
            for (int i = 0; i < numOfWorkers; i++)
            {
                _wokersPool.Add(new Worker(++_workerID));
            }
        }

        public Worker GetNextWorker()
        {
            if (_position >= _wokersPool.Count)
                return null;

            return _wokersPool[_position++];
        }
    }
}
