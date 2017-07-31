using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Events;
using TaskSimulation.Simulator.Workers;

namespace TaskSimulation.Results
{
    public class WorkersStatisticsGrouping : ISimulatable
    {
        private List<Worker> _workers;
        private Dictionary<Worker, WorkerData> _queueSum;
        private Dictionary<Worker, WorkerData> _waitSum;

        public WorkersStatisticsGrouping()
        {
            _workers = new List<Worker>();
            _queueSum = new Dictionary<Worker, WorkerData>();
            _waitSum = new Dictionary<Worker, WorkerData>();
        }

        private class WorkerData
        {
            public double Queue { get; set; } = 0;
            public double StartTime { get; set; } = 0;
            public double EndTime { get; set; } = 0;
            public double Sum { get; set; } = 0;

            public void Reset()
            {
                StartTime = 0;
                EndTime = 0;
            }
        }

        public void Update(WorkerArrivalEvent @event)
        {
            _workers.Add(@event.Worker);
            _queueSum.Add(@event.Worker, new WorkerData());
            _waitSum.Add(@event.Worker, new WorkerData());
        }

        public void Update(WorkerLeaveEvent @event)
        {
        }

        public void Update(TaskFinishedEvent @event)
        {
            var worker = @event.Worker;
            var task = @event.Task;
            var time = @event.ArriveTime;

            _queueSum[worker].EndTime = time;
            _queueSum[worker].Sum += _queueSum[worker].Queue + (_queueSum[worker].EndTime - _queueSum[worker].StartTime);
            _queueSum[worker].Reset();
            _queueSum[worker].Queue--;

            // Worker is doing nothing
            if (_queueSum[worker].Queue == 0)
            {
                _waitSum[worker].StartTime = time;
            }
        }

        public void Update(TaskArrivalEvent @event)
        {
            var task = @event.Task;
            var time = @event.ArriveTime;

            var existingWorker = task.GetWorker();

            // Worker already assigned to the task (before the OnAddedToWorker registered)
            if (existingWorker != null)
            {
                _queueSum[existingWorker].Queue++;
                _queueSum[existingWorker].StartTime = time;
            }

            // Not added to worker yet
            task.OnAddedToWorker += worker =>
            {
                try
                {
                    if (_queueSum[worker].Queue == 0)
                    {
                        _waitSum[worker].EndTime = time;
                        _waitSum[worker].Sum += _queueSum[worker].EndTime - _queueSum[worker].StartTime;
                        _waitSum[worker].Reset();
                    }

                    _queueSum[existingWorker].Queue++;
                    _queueSum[existingWorker].StartTime = time;
                }
                catch (Exception e)
                {
                    Log.Err("ERR");
                }
            };
        }

        public Dictionary<Worker, double> GetAvarageUtilizationPerWorker()
        {
            var dict = new Dictionary<Worker, double>();
            foreach (var worker in _workers)
            {
                dict.Add(worker, worker.Statistics.BusyTime / SimulateServer.SimulationClock);
            }

            return dict;
        }

        public Dictionary<Worker, double> GetAvarageQueuePerWorker()
        {
            var dict = new Dictionary<Worker, double>();
            foreach (var worker in _workers)
            {
                dict.Add(worker, _queueSum[worker].Sum / SimulateServer.SimulationClock);
            }

            return dict;
        }

        public Dictionary<Worker, double> GetAvarageWaitingTimePerWorker()
        {
            var dict = new Dictionary<Worker, double>();
            foreach (var worker in _workers)
            {
                dict.Add(worker, _waitSum[worker].Sum / SimulateServer.SimulationClock);
            }

            return dict;
        }
    }
}
