using System;
using System.Collections.Generic;
using TaskSimulation.ChooseAlgorithms;
using TaskSimulation.Distribution;
using TaskSimulation.Simulator.Events;
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
        private readonly IWorkersGenerator _workersGenerator;
        //public event Action<Worker> OnNewWorkerArrived;

        public WorkersJournal(int initialNumOfWorkers)
        {
            _workersGenerator = new WorkersGenerator();
            _activeWorkers = new List<Worker>();

            for (var i = 0; i < initialNumOfWorkers; i++)
                AddNewWorker();
        }

        private void AddNewWorker()
        {
            //var newWorker = new Worker();
            var newWorker = _workersGenerator.GetNextWorker();
            if (newWorker == null)
            {
                Log.Err($"Error!! No more available workers!");
                return;
            }

            _activeWorkers.Add(newWorker);

        }

        public List<Worker> AssignTask(Task task)
        {
            if (_activeWorkers.Count <= 0)
                return null;

            var chooseAlgo = new ChooseHighestGrade(); 
            var workers = chooseAlgo.ChooseWorkers(_activeWorkers, 1);  // TODO assumption: chose only 1 worker

            // Assign the task to each worker
            workers?.ForEach(worker =>
            {
                worker?.Assign(task);
                Log.Event($"{worker} has been assigned to {task}");
            });

            return workers;
        }
         

        public List<Worker> ActiveWorkers
        {
            get { return _activeWorkers; }
        }

        public void Update(WorkerArrivalEvent @event)
        {
            var worker = _workersGenerator.GetNextWorker();

            if (worker == null)
            {
                Log.Err("No more workers to create");
                return;
            }
            @event.Worker = worker;
            
            _activeWorkers.Add(worker);

            var simClock = SimulateServer.SimulationClock;
            worker.Statistics.StartAt = simClock;

            var finishIn = SimDistribution.I.WorkerLeaveTime.Sample();
            @event.EventMan.AddEvent(new WorkerLeaveEvent(worker, simClock + finishIn));
        }

        public void Update(WorkerLeaveEvent @event)
        {
            // TODO for each worker task in the list reassign them
            var worker = @event.Worker;

            worker.Statistics.EndAt = SimulateServer.SimulationClock;

            _activeWorkers.Remove(worker);
        }

        public void Update(TaskArrivalEvent @event)
        {
            var task = @event.Task;

            var assignedWorkers = AssignTask(task);

            assignedWorkers.ForEach(worker =>
            {
                // Assumption the simulator clock is always updated to current time
                var simClock = SimulateServer.SimulationClock;

                // TODO calc using worker's data
                var finishIn = SimDistribution.I.ResponseTime.Sample();
                @event.EventMan.AddEvent(new TaskFinishedEvent(task, worker, simClock + finishIn));
            });
        }

        public void Update(TaskFinishedEvent @event)
        {
            var worker = @event.Worker;
            var task = @event.Task;

            worker.RemoveTask(task);
        }

    }
}
