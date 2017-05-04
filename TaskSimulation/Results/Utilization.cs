using System;
using System.Collections.Generic;
using System.Linq;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Events;
using Task = TaskSimulation.Simulator.Task;

namespace TaskSimulation.Results
{
    public class Utilization : ISimulatable
    {
        private readonly List<Task> _tasks;
        private readonly List<Worker> _workers;

        public Utilization()
        {
            _tasks = new List<Task>();
            _workers = new List<Worker>();
        }

        public void AddWorkers(List<Worker> workers)
        {
            _workers.AddRange(workers);
        }

        public void AddTasks(List<Task> tasks)
        {
            _tasks.AddRange(tasks);
        }

        public void Update(TaskArrivalEvent @event)
        {
            _tasks.Add(@event.Task);
        }

        public void Update(TaskFinishedEvent @event) { }

        public void Update(WorkerArrivalEvent @event)
        {
            _workers.Add(@event.Worker);
        }

        public void Update(WorkerLeaveEvent @event) { }


        public int GetNumberOfFinishedTasks()
        {
            var totalTasksFinished = _tasks.Count(t => t.EndTime != Task.NOT_STARTED);
            Log.I($"Total Task finished = {totalTasksFinished}");
            return totalTasksFinished;
        }

        public int GetNumberOfTotalTasks()
        {
            var totalTasks = _tasks.Count;
            Log.I($"Total Task = {totalTasks}");
            return totalTasks;
        }

        public void FinalizeUtilization()
        {
            
        }

        public double GetTotalWorkersUtilization()
        {
            var workersWorked = _workers.Sum(w => w.Statistics.BusyTime);
            var workersTotalTime = _workers.Sum(w => w.Statistics.TotalTime);
            var workerUtilization = workersWorked / workersTotalTime;
            Log.I($"Workers utilization is {workersWorked}/{workersTotalTime} = {workerUtilization*100:N2}%");
            return workerUtilization;
        }

        /// <summary>
        /// (Total time the workers worked/Number of workers)/Simulation Run time = Utiliztion
        /// If all the workers were working all the time, the system utilization has been 100%
        /// </summary>
        /// <returns></returns>
        public double GetSystemUtilization()
        {
            var systemWorked = _workers.Sum(w =>w.Statistics.BusyTime);
            var systemUtilization = (systemWorked /_workers.Count)/SimulateServer.SimulatorMaxRunTime;
            Log.I($"System utilization is: " +
                  $"({systemWorked}/{_workers.Count})/{SimulateServer.SimulatorMaxRunTime} = {systemUtilization * 100:N2}%");
            return systemUtilization;
        }

        public double TaskWereInWaitList()
        {
            var totalWaitTime = _tasks.Sum(t => t.StartTime - t.CreatedTime);
            var totalSystemTime = SimulateServer.SimulatorMaxRunTime * _tasks.Count;
            var total = totalWaitTime / totalSystemTime;
            Log.I($"Task were waiting: " +
                  $"{totalWaitTime}/{totalSystemTime} = {total * 100:N2}%");
            return total;
        }
    }
}
