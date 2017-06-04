using System.Collections.Generic;
using TaskSimulation.ChooseAlgorithms;
using TaskSimulation.Distribution;
using TaskSimulation.Simulator.Events;
using TaskSimulation.Simulator.Tasks;

namespace TaskSimulation.Simulator.Workers
{
    /// <summary>
    /// Workers journal is responsible for the workers management
    /// </summary>
    class WorkersJournal : ISimulatable
    {
        private readonly List<Worker> _activeWorkers;
        private readonly IWorkersGenerator _workersGenerator;
        private const int NUM_OF_WORKERS_ON_TASK = 1;   // todo move to settings file
        private readonly IChooseWorkerAlgo _chooseAlgo;

        public List<Worker> ActiveWorkers => _activeWorkers;

        public WorkersJournal()
        {
            _workersGenerator = new WorkersGenerator(SimDistribution.I.WorkersQualityDistribution);
            _activeWorkers = new List<Worker>();
            //_chooseAlgo = new ChooseHighestGrade();
            _chooseAlgo = new ChooseLowestGrade();
        }

        public void AssignTask(Task task)
        {
            if (_activeWorkers.Count <= 0)
                return;

            var workers = _chooseAlgo.ChooseWorkers(_activeWorkers, NUM_OF_WORKERS_ON_TASK);

            // Assign the task to each worker
            workers?.ForEach(worker =>
            {
                Log.Event($"Adding {task} to {worker}");
                worker?.AddTask(task);
            });
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

            var finishIn = SimDistribution.I.WorkerLeaveRate.Sample();
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

            AssignTask(task);

        }

        public void Update(TaskFinishedEvent @event)
        {
            var worker = @event.Worker;
            var task = @event.Task;

            if (!worker.IsOnline()) return;

            worker.RemoveTask(task);
        }

    }
}
