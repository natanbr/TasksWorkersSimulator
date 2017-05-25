using System;
using System.Collections.Generic;
using System.Linq;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Events;
using TaskSimulation.Simulator.Tasks;
using TaskSimulation.Simulator.Workers;

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

        private double GetWorkersWorkedTime()
        {
            var sumWorkersWorkedTime = _workers.Sum(w =>
            {
                // The last not finished task
                var lastTask = (w.IsWorking && w.IsOnline()) ? SimulateServer.SimulationClock - w.GetCurrentTask().StartTime : 0;

                return w.Statistics.BusyTime + lastTask;
            });

            return sumWorkersWorkedTime;
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
            var sumWorkersBusy = GetWorkersWorkedTime();
            Log.I($"(S=Start time, E=End time, B=Busy time, T=Total existing time");
            var workersTotalTime = _workers.Sum(w =>
            {
                Log.I($"{w} (S:{w.Statistics.StartAt,-6:#0.###}, E:{w.Statistics.EndAt,-6:#0.###}) B:{w.Statistics.BusyTime,-6:#0.###} T:{w.Statistics.TotalTime,-6:#0.###}");
                return w.Statistics.TotalTime;
            });

            var workerUtilization = sumWorkersBusy / workersTotalTime;
            Log.I($"Workers utilization is (sum workers work time)/(sum workers work time)= workerUtilization");
            Log.I($"Workers utilization is {sumWorkersBusy}/{workersTotalTime} = {workerUtilization*100:N2}%");
            return workerUtilization;
        }

        /// <summary>
        /// (Total time the workers worked/Number of workers)/Simulation Run time = Utiliztion
        /// If all the workers were working all the time, the system utilization has been 100%
        /// </summary>
        /// <returns></returns>
        public double GetSystemUtilization()
        {
            var sumWorkersWorkedTime = GetWorkersWorkedTime();

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
