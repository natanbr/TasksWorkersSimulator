using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Events;
using TaskSimulation.Utiles;

namespace TaskSimulation.Results
{
    public class SystemUtilizationStatistics2 : ISimulatable
    {
        public readonly List<UtilizationData> Data;
        private readonly List<Tuple<double, double>> _avarageSystemUtilization;
        private readonly List<Tuple<double, double>> _systemUtilization;


        public List<Tuple<double, double>> GetSystemUtilization()
        {
            return Data.Select(v => new Tuple<double, double>(v.Time, v.Utilization)).ToList();
        }

        public List<Tuple<double, double>> GetAvarageSystemUtilization()
        {
            Data.CalculateUtilization(SimulateServer.SimulationClock);

            for (var i = 1; i < Data.Count; i++)
            {
                var d = Data[i];
                var utilizationSum = (double)0;
                var sum = (double)0;

                for (var j = 1; j <= i; j++)
                {
                    var v = Data[j];
                    var vPre = Data[j-1];
                    var deltaTime = v.Time - vPre.Time;
                    utilizationSum += vPre.Utilization * deltaTime * vPre.WorkingWorkers;
                    sum += deltaTime* vPre.TotalWorkers;
                }

                _avarageSystemUtilization.Add(d.Time, utilizationSum / sum);
            }

            return _avarageSystemUtilization;
        }

        public SystemUtilizationStatistics2()
        {
            _avarageSystemUtilization = new List<Tuple<double, double>>();
            _avarageSystemUtilization.Add(0,0);
            _systemUtilization = new List<Tuple<double, double>>();
            _systemUtilization.Add(0,0);

            Data = new List<UtilizationData>
            {
                new UtilizationData(0, 0, 0)
            };
        }

        public void Update(TaskArrivalEvent @event)
        {
            var time = @event.ArriveTime;
            var task = @event.Task;

            var existingWorker = task.GetWorker();

            // Worker already assigned to the task (before the OnAddedToWorker registered)
            if (existingWorker?.GetNumberOfTasks() == 1)
            {
                Data.AddWorkingWorker(time);
            }

            // Not added to worker yet
            task.OnAddedToWorker += worker =>
            {
                // Worker moved from 0 tasks to 1 task (now it is working)
                if (worker.GetNumberOfTasks() == 1)
                {
                    Data.AddWorkingWorker(time);
                }
            };
        }

        public void Update(TaskFinishedEvent @event)
        {
            var worker = @event.Worker;
            var time = @event.ArriveTime;

            // Worker finished his last tasks now it has 0 tasks
            if (worker.GetNumberOfTasks() == 0)
            {
                Data.RemoveWorkingWorker(time);
            }
        }

        public void Update(WorkerArrivalEvent @event)
        {
            var time = @event.ArriveTime;

            Data.AddWorker(time);
        }

        public void Update(WorkerLeaveEvent @event)
        {
            var time = @event.ArriveTime;
            var worker = @event.Worker;

            if (worker.GetNumberOfTasks() > 0)
                Data.RemoveWorkingWorker(time);

            Data.RemoveWorker(time);
        }
    }

    public class UtilizationData
    {
        public double Time;
        public int WorkingWorkers;
        public int TotalWorkers;
        public double Utilization;

        public UtilizationData(double time, int workingWorkers, int totalWorkers)
        {
            Time = time;
            WorkingWorkers = workingWorkers;
            TotalWorkers = totalWorkers;
        }
    }

    /// <summary>
    /// TODO think of create ways to refactor this code
    /// </summary>
    public static class UtilizationDataUtils
    {
        public static void CalculateUtilization(this List<UtilizationData> data, double time)
        {
            var prev = data.Last();

            if (prev.Time.Equals(time))
            {
                prev.Utilization = (double)prev.WorkingWorkers / prev.TotalWorkers;
            }
            else
            {
                var newData = new UtilizationData(time, prev.WorkingWorkers, prev.TotalWorkers);
                newData.Utilization = (double)prev.WorkingWorkers / prev.TotalWorkers;
                data.Add(newData);
            }
        }

        public static void AddWorker(this List<UtilizationData> data, double time)
        {
            var prev = data.Last();

            if (prev.Time.Equals(time))
            {
                prev.TotalWorkers++;
            }
            else
            {
                var newData = new UtilizationData(time, prev.WorkingWorkers, prev.TotalWorkers+1);
                data.Add(newData);
            }

            data.CalculateUtilization(time);
        }

        public static void AddWorkingWorker(this List<UtilizationData> data, double time)
        {
            var prev = data.Last();

            if (prev.Time.Equals(time))
            {
                prev.WorkingWorkers++;
            }
            else
            {
                var newData = new UtilizationData(time, prev.WorkingWorkers+1, prev.TotalWorkers);
                data.Add(newData);
            }

            data.CalculateUtilization(time);
        }

        public static void RemoveWorker(this List<UtilizationData> data, double time)
        {
            var prev = data.Last();

            if (prev.Time.Equals(time))
            {
                prev.TotalWorkers--;
            }
            else
            {
                var newData = new UtilizationData(time, prev.WorkingWorkers, prev.TotalWorkers-1);
                data.Add(newData);
            }

            data.CalculateUtilization(time);
        }

        public static void RemoveWorkingWorker(this List<UtilizationData> data, double time)
        {
            var prev = data.Last();

            if (prev.Time.Equals(time))
            {
                prev.WorkingWorkers--;
            }
            else
            {
                var newData = new UtilizationData(time, prev.WorkingWorkers-1, prev.TotalWorkers);
                data.Add(newData);
            }

            data.CalculateUtilization(time);
        }
    }
}