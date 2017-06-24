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
            _systemUtilization = new List<Tuple<double, double>>();//{new Tuple<double, double>(0, 0) };

            _avarageSystemUtilization = new List<Tuple<double, double>>();//{new Tuple<double, double>(0, 0)};
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
                    _lastNumberOfWorkingWorkers++; // Must be before calc if first time
                    CalcAndAdd(@event.ArriveTime, newValue);
                }
            }
            // Not added to worker yet
            task.OnAddedToWorker += worker =>
            {
                // Worker moved from 0 tasks to 1 task (now he is working)
                if (worker.GetNumberOfTasks() == 1)
                {
                    double newValue;

                    if (_lastNumberOfWorkers == 0)
                        newValue = 1;
                    else
                        newValue = (double) (_lastNumberOfWorkingWorkers + 1)/_lastNumberOfWorkers;

                    CalcAndAdd(@event.ArriveTime, newValue);

                    _lastNumberOfWorkingWorkers++; // Must be after calc, average calc uses the old value

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
                CalcAndAdd(@event.ArriveTime, newValue);
                _lastNumberOfWorkingWorkers--;
            }
        }
    

        public void Update(WorkerArrivalEvent @event)
        {
            var newValue = (double)(_lastNumberOfWorkingWorkers) / (_lastNumberOfWorkers + 1);
            CalcAndAdd(@event.ArriveTime, newValue);
            _lastNumberOfWorkers++;
        }

        public void Update(WorkerLeaveEvent @event)
        {
            var newValue = (double) (_lastNumberOfWorkingWorkers)/(_lastNumberOfWorkers - 1);
            CalcAndAdd(@event.ArriveTime, newValue);
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

        public void AddLastValue()
        {
            var last = _systemUtilization.Last();
            CalcAndAdd(SimulateServer.SimulationClock, last.Item2);
        }

        private void CalcAndAdd(double time, double newValue)
        {
            _systemUtilization.AddUnique(time, newValue);

            // If the value is the first (or overrides the first)
            if (_avarageSystemUtilization.Count == 0  || (_avarageSystemUtilization.Count == 1 && _avarageSystemUtilization[0].Item1.Equals(time)))
            {
                _avarageSystemUtilization.AddUnique(time, newValue);
                return;
            }

            var last = _avarageSystemUtilization.Last();

            if (last.Item1.Equals(time))
                last = _avarageSystemUtilization.GetFromEnd(1);

            var oldAvarage = last.Item2;


            time = time == 0 ? 1 : time; //In case time is 0
            var deltaTime = time - _lastRecord;
            // Using the last value (not the new one)
            //           old * (last number of workers * previous time) + (system utilization * delta time *  last Number Of Working Workers)
            // Avarage = ------------------------------------------------------------------------------------------------------------------------------
            //                                           last number of all workers * total time
            var newAvarage = LMath.Average(oldAvarage, _lastNumberOfWorkers * _lastRecord, _systemUtilization.GetFromEnd(1).Item2 * deltaTime * _lastNumberOfWorkingWorkers, _lastNumberOfWorkers * time);
            _avarageSystemUtilization.AddUnique(time, newAvarage);
            _lastRecord = time;
        }

    }
}