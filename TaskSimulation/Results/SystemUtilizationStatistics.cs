using System;
using System.Collections.Generic;
using System.Linq;
using TaskSimulation.Distribution;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Events;
using TaskSimulation.Simulator.Workers;
using TaskSimulation.Utiles;

namespace TaskSimulation.Results
{
    public class SystemUtilizationStatistics : ISimulatable
    {
        private readonly List<Tuple<double, double>> _avarageSystemUtilization;
        private readonly List<Tuple<double, double>> _systemUtilization;

        private int _lastNumberOfWorkingWorkers = 0;
        private int _lastNumberOfWorkers = 0;
        private double _lastRecord = 0;

        public SystemUtilizationStatistics()
        {
            _systemUtilization = new List<Tuple<double, double>>(){
                new Tuple<double, double>(0, 0)
            };

            _avarageSystemUtilization = new List<Tuple<double, double>>(){
                new Tuple<double, double>(0, 0)
            };
        }

        public void Update(TaskArrivalEvent @event)
        {
            var task = @event.Task;

            var existingWorker = task.GetWorker();
            if (task.GetWorker() != null)
            {
                if (existingWorker.GetNumberOfTasks() == 1)
                {
                    var newValue = (double)(_lastNumberOfWorkingWorkers + 1) / _lastNumberOfWorkers;
                    _lastNumberOfWorkingWorkers++;

                    CalcAndAdd(@event, newValue);
                }
            }
            // Not added to worker yet
            task.OnAddedToWorker += worker =>
            {
                // Worker moved from 0 tasks to 1 task (now he is working)
                if (worker.GetNumberOfTasks() == 1)
                {
                    var newValue = (double) (_lastNumberOfWorkingWorkers + 1)/_lastNumberOfWorkers;
                    _lastNumberOfWorkingWorkers++;

                    CalcAndAdd(@event, newValue);
                }
            };
        }

       
        public void Update(TaskFinishedEvent @event)
        {
            var worker = @event.Worker;

            // Worker finished his last tasks now he has 0 tasks
            if (worker.GetNumberOfTasks() == 0)
            {
                var newValue = (double) (_lastNumberOfWorkingWorkers - 1)/_lastNumberOfWorkers;
                _lastNumberOfWorkingWorkers--;
                CalcAndAdd(@event, newValue);
            }
        }
    

        public void Update(WorkerArrivalEvent @event)
        {
            var newValue = (double)(_lastNumberOfWorkingWorkers) / (_lastNumberOfWorkers + 1);
            CalcAndAdd(@event, newValue);
            _lastNumberOfWorkers++;
        }

        public void Update(WorkerLeaveEvent @event)
        {
            var newValue = (double) (_lastNumberOfWorkingWorkers)/(_lastNumberOfWorkers - 1);
            CalcAndAdd(@event, newValue);
            _lastNumberOfWorkers--;
        }

        public List<Tuple<double, double>> GetAvarageSystemUtilization()
        {
            return _avarageSystemUtilization;
        }

        public List<Tuple<double, double>> GetSystemUtilization()
        {
            return _systemUtilization;
        }

        private void CalcAndAdd(AEvent @event, double newValue)
        {
            _systemUtilization.Add(@event.ArriveTime, newValue);
            var oldAvarage = _avarageSystemUtilization.Last().Item2;
            var deltaTime = @event.ArriveTime - _lastRecord;
            var time = @event.ArriveTime == 0 ? 1 : @event.ArriveTime; //In case time is 0
            var newAvarage = LMath.Average(oldAvarage, _lastNumberOfWorkers * _lastRecord, newValue * deltaTime, _lastNumberOfWorkers * time);
            _avarageSystemUtilization.Add(@event.ArriveTime, newAvarage);
            _lastRecord = @event.ArriveTime;
        }

    }
}