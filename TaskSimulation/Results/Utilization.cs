using System;
using System.Collections.Generic;
using System.Linq;
using TaskSimulation.Simulator;
using Task = TaskSimulation.Simulator.Task;

namespace TaskSimulation.Results
{
    public class Utilization : ISimulatable
    {
        public List<Task> _tasks;
        public List<Worker> _workers;

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

        public Utilization()
        {
            _tasks = new List<Task>();
            _workers = new List<Worker>();
        }

        public void Visit(Task task)
        {
            _tasks.Add(task);
        }

        public void Visit(Worker worker)
        {
            _workers.Add(worker);
        }

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

        public double GetTotalWorkersUtilization()
        {
            var workersWorked = _workers.Sum(w => w.Utilization.BusyTime);
            var workersTotalTime = _workers.Sum(w => w.Utilization.TotalTime);
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
            var systemWorked = _workers.Sum(w => w.Utilization.BusyTime);
            var systemUtilization = (systemWorked /_workers.Count)/SimulateServer.SimulatorMaxRunTime;
            Log.I($"System utilization is: " +
                  $"({systemWorked}/{_workers.Count})/{SimulateServer.SimulatorMaxRunTime} = {systemUtilization * 100:N2}%");
            return systemUtilization;
        }


    }
}
