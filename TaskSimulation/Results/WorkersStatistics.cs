using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Events;
using TaskSimulation.Simulator.Workers;
using TaskSimulation.Utiles;

namespace TaskSimulation.Results
{
    public class WorkersStatistics : ISimulatable
    {
        private readonly List<Tuple<double, double>> _avarageWorkersEfficiency;

        private readonly List<Tuple<double, int>> _workersCount;
        private readonly List<Tuple<double, int>> _workersLeft;

        private readonly Dictionary<Worker, WorkerData> _workers;

        public WorkersStatistics()
        {
            _avarageWorkersEfficiency = new List<Tuple<double, double>>
            {
                new Tuple<double, double>(0, 0)
            };

            _workersCount = new List<Tuple<double, int>>
            {
                new Tuple<double, int>(0, 0)
            };

            _workersLeft = new List<Tuple<double, int>>
            {
                new Tuple<double, int>(0, 0)
            };

            _workers = new Dictionary<Worker, WorkerData>();
        }

        public double GetAvarageWorkersEfficiency()
        {
            Log.D("Get Avarage Workers' Efficiency: (time, average) \n" + Print(_avarageWorkersEfficiency));
            return _avarageWorkersEfficiency.Last().Item2;
        }

        public double GetNumberOfTotalWorkers()
        {
            Log.D("Get Number Of Active Workers \n" + Print(_workersCount));
            return _workersCount.Last().Item2;
        }

        /// <summary>
        /// Get workers worked time, including last moments of the simulation time
        /// </summary>
        /// <returns></returns>
        public double GetWorkersWorkedTime()
        {
            var sumWorkersWorkedTime = _workers.Keys.Sum(w =>
            {
                // The last not finished task
                var lastTask = (w.IsWorking && w.IsOnline()) ? SimulateServer.SimulationClock - w.GetCurrentTask().StartTime : 0;

                return w.Statistics.BusyTime + lastTask;
            });

            return sumWorkersWorkedTime;
        }
        public void Update(TaskFinishedEvent @event)
        {
            var task = @event.Task;
            var worker = @event.Worker;

            var numberOfWorkersBefore = _workers.Count;
            var data = GetWorkersData(worker);

            var workersOldAvarage = data.AvarageEfficiency;
            var prevNumOfTasks = data.NumberOfTasksFinished;
            var newValue = task.EndTime - task.StartTime;
            data.NumberOfTasksFinished++;

            // Add value to avarage
            var newWorkerAverageEfficiency = LMath.Average(workersOldAvarage, prevNumOfTasks, newValue, data.NumberOfTasksFinished);
            var deltaInWorkersAvarage = newWorkerAverageEfficiency - data.AvarageEfficiency;
            data.AvarageEfficiency = newWorkerAverageEfficiency;

            var prevAvarage = _avarageWorkersEfficiency.Last().Item2;
            var newAvarage = LMath.Average(prevAvarage, numberOfWorkersBefore, deltaInWorkersAvarage, _workers.Count);

            _avarageWorkersEfficiency.Add(new Tuple<double, double>(task.EndTime, newAvarage));
        }

        private WorkerData GetWorkersData(Worker worker)
        {
            var hasKey = _workers.ContainsKey(worker);
            if (!hasKey)
            {
                var workerData = new WorkerData();
                _workers.Add(worker, workerData);
            }

            return _workers[worker];
        }

        public void Update(TaskArrivalEvent @event)
        {
        }

        public void Update(WorkerArrivalEvent @event)
        {
            _workersCount.Add(new Tuple<double, int>(@event.ArriveTime, _workersCount.Last().Item2 + 1));
        }

        public void Update(WorkerLeaveEvent @event)
        {
            _workersCount.Add(new Tuple<double, int>(@event.ArriveTime, _workersCount.Last().Item2 - 1));
            _workersLeft.Add(new Tuple<double, int>(@event.ArriveTime, _workersCount.Last().Item2 + 1));
        }

        public class WorkerData
        {
            public double AvarageEfficiency { get; set; } = 0;
            public int NumberOfTasksFinished { get; set; } = 0;
        }


        public string Print<T, M>(List<Tuple<T, M>> workingSet)
        {
            var sb = new StringBuilder();

            foreach (var tuple in workingSet)
            {
                sb.AppendLine($"x: {tuple.Item1}, value {tuple.Item2,-7:#0.###}");
            }
            return sb.ToString();
        }
    }
}