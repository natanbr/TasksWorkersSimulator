using System.Collections.Generic;

namespace TaskSimulation.Simulator.Workers
{
    public class WorkersGenerator : IWorkersGenerator
    {
        private long _workerID = 0;
        private readonly List<Worker> _wokersPool;
        private int _position = 0;
        private const int INITIAL_POOL_SIZE = 10;          // todo move to settings file
        private WorkersQualityDistribution _qualities;

        public WorkersGenerator(WorkersQualityDistribution qualities)
        {
            _wokersPool = new List<Worker>();
            _qualities = qualities;
            AddWorkers(INITIAL_POOL_SIZE);
        }

        private void AddWorkers(int numOfWorkers)
        {
            for (int i = 0; i < numOfWorkers; i++)
            {
                var workerQualities = _qualities.GenerateQualies();
                _wokersPool.Add(new Worker(++_workerID, workerQualities));
            }
        }

        public Worker GetNextWorker()
        {
            if (_position >= _wokersPool.Count)
                AddWorkers(_wokersPool.Count + 1);

            return _wokersPool[_position++];
        }
    }
}
