using System;
using System.Collections.Generic;
using System.Linq;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Events;
//using Task = TaskSimulation.Simulator.Task;

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
            var sumWorkersWorkedTime = _workers.Sum(w =>w.Statistics.BusyTime);
            var systemUtilization = (sumWorkersWorkedTime /_workers.Count)/SimulateServer.SimulationClock;

            Log.I($"System utilization is: (sum Workers Worked Time/#workers)/Simulation Final Time");
            Log.I($"System utilization is: " +
                  $"({sumWorkersWorkedTime}/{_workers.Count})/{SimulateServer.SimulationClock} = {systemUtilization * 100:N2}%");
            return systemUtilization;
        }

        public double TaskWereInWaitList()
        {
            var sumTasksWaitTime = _tasks.Sum(t =>
            {
                if (t.StartTime != -1)
                    return t.StartTime - t.CreatedTime;

                // Task was not finished
                return SimulateServer.SimulationClock - t.CreatedTime;
            });

            var sumTasksExistsTime = _tasks.Sum(t =>
            {
                Log.I($"{t} - C:{t.CreatedTime}  S:{t.StartTime}  F:{t.EndTime}");

                if (t.EndTime != -1)
                    return t.EndTime - t.CreatedTime;
                
                // Task was not finished
                return SimulateServer.SimulationClock - t.CreatedTime;
            });

            var total = sumTasksWaitTime / sumTasksExistsTime;

            Log.I($"Task were waiting: (tasks sum wait time) / (sum of time tasks exists)");
            Log.I($"Task were waiting: " +
                  $"{sumTasksWaitTime}/{sumTasksExistsTime} = {total * 100:N2}%");
            return total;
        }
    }
}
