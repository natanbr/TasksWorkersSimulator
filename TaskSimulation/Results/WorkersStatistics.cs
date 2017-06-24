using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Events;
using TaskSimulation.Simulator.Tasks;
using TaskSimulation.Simulator.Workers;
using TaskSimulation.Utiles;

namespace TaskSimulation.Results
{
    public class WorkersStatistics : ISimulatable
    {
        private readonly List<Tuple<double, double>> _avarageWorkersEfficiency;

        private readonly List<Tuple<double, int>> _workersCount;
        private readonly List<Tuple<double, int>> _workersLeft;
        private Dictionary<Worker, Utilization.WorkerData> _workers;
        private int _numberOfWorkersBefore;

        public WorkersStatistics(Dictionary<Worker, Utilization.WorkerData> workers)
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

            _workers = workers;//new Dictionary<Worker, Utilization.WorkerData>();
        }

        #region Updates

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

        public void Update(TaskFinishedEvent @event)
        {
            var task = @event.Task;
            var worker = @event.Worker;

            UpdateAverageEfficiency(worker, task);
        }

        #endregion


        public double GetLastAvarageWorkersEfficiency()
        {
            Log.D("Get Avarage Workers' Efficiency: (time, average) \n" + Print(_avarageWorkersEfficiency));
            return _avarageWorkersEfficiency.Last().Item2;
        }

        public List<Tuple<double, double>> GetAvarageWorkersEfficiency()
        {
            return _avarageWorkersEfficiency;
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


        /// <summary>
        /// The Aveage efficiency is the avarage of the wokres efficiency: sum the workers avage/ number of workers
        /// Calculate the workers avarage efficiency: sum the working time/ number of tasks
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="task"></param>
        private void UpdateAverageEfficiency(Worker worker, Task task)
        {
            if (_numberOfWorkersBefore == 0)
                _numberOfWorkersBefore = _workers.Count;

            var data = GetWorkersData(worker);

            var workersOldAvarage = data.AvarageEfficiency;
            //var prevNumOfTasks = data.NumberOfTasksFinished; 
            var newValue = task.EndTime - task.StartTime;
            var prevTime = data.TimeAlive;

            // Add value to avarage
            var newWorkerAverageEfficiency = LMath.Average(workersOldAvarage, prevTime, newValue, task.EndTime);
            var deltaInWorkersAvarage = newWorkerAverageEfficiency - data.AvarageEfficiency;
            var prevAvarage = _avarageWorkersEfficiency.Last().Item2;
            var newAvarage = LMath.Average(prevAvarage, _numberOfWorkersBefore, deltaInWorkersAvarage, _workers.Count);

            if (newAvarage > 1)
                Log.Err("ASDFASDF");

            data.AvarageEfficiency = newWorkerAverageEfficiency;
            data.TimeAlive = task.EndTime;
            data.NumberOfTasksFinished++;
            _numberOfWorkersBefore = _workers.Count;

            _avarageWorkersEfficiency.Add(new Tuple<double, double>(task.EndTime, newAvarage));
        }

        private Utilization.WorkerData GetWorkersData(Worker worker)
        {
            var hasKey = _workers.ContainsKey(worker);
            if (hasKey)
            {
                return _workers[worker];
            }

            return null;
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

