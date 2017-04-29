using System;
using System.Collections.Generic;
using TaskSimulation.ChooseAlgorithms;
using TaskSimulation.Distribution;
using TaskSimulation.Workers;

namespace TaskSimulation.Simulator
{
    /// <summary>
    /// Workers journal is responsible for the workers management
    /// 
    /// </summary>
    class WorkersJournal : ISimulatable
    {
        private readonly List<Worker> _activeWorkers;
        private readonly List<Worker> _idleWorkers;
        private readonly IWorkersGenerator _workersGenerator;
        public event Action<Worker> OnNewWorkerArrived;

        public WorkersJournal(int initialNumOfWorkers)
        {
            _workersGenerator = new WorkersGenerator();
            _activeWorkers = new List<Worker>();
            _idleWorkers = new List<Worker>();

            for (var i = 0; i < initialNumOfWorkers; i++)
                AddNewWorker();
        }

        public void Update()
        {
            // Generate new Worker
            var addWorker= DistFactory.WorkerArrivalRate.Test();

            //DistFactory.WorkerArrivalRate.PrintLastCalc("Add worker");

            if (addWorker)
            {
                var worker = AddNewWorker();

                CallNewWorkerArrived(worker);
            }

            foreach (var worker in _activeWorkers)
            {
                worker.Update();
            }

            // iterate over the workers and update the status
            UpdateWorkers();
        }

        private Worker AddNewWorker()
        {
            //var newWorker = new Worker();
            var newWorker = _workersGenerator.GetNextWorker();
            if (newWorker == null)
            {
                Log.Err($"Error!! No more available workers!");
                return null;
            }

            _activeWorkers.Add(newWorker);

            Log.Event($"W> Adding new {newWorker}");

            return newWorker;
        }

        private void UpdateWorkers()
        {
            for (var i = _activeWorkers.Count - 1; i >= 0; i--)
            {
                var worker = _activeWorkers[i];
                if (!worker.WorkerStatus)
                {
                    _activeWorkers.RemoveAt(i);
                    _idleWorkers.Add(worker);
                }
            }

            for (var i = _idleWorkers.Count - 1; i >= 0; i--)
            {
                var worker = _idleWorkers[i];
                if (worker.WorkerStatus)
                {
                    _idleWorkers.RemoveAt(i);
                    _activeWorkers.Add(worker);
                }
            }
        }

        public void AssignTask(Task task)
        {
            if (_activeWorkers.Count <= 0)
                return;

            var chooseAlgo = new ChooseHighestGrade(); 
            var workers = chooseAlgo.ChooseWorkers(_activeWorkers, 1);  // TODO assumption: chose only 1 worker

            // Assign the task to each worker
            workers.ForEach(worker =>
            {
                worker?.Assign(task);
                Log.Event($"{worker} has been assigned to {task}");
            });
        }
         
        protected virtual void CallNewWorkerArrived(Worker worker)
        {
            OnNewWorkerArrived?.Invoke(worker);
        }

        public List<Worker> ActiveWorkers
        {
            get { return _activeWorkers; }
        }

    }
}
