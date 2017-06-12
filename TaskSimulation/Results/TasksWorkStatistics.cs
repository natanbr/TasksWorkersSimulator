using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Events;
using TaskSimulation.Simulator.Tasks;
using TaskSimulation.Utiles;

namespace TaskSimulation.Results
{
    public class TasksWorkStatistics : ISimulatable
    {
        private readonly List<Tuple<double,double>> _averageProcessingTime;
        private readonly List<Tuple<double, double>> _avarageWatingTime;
        private readonly List<Tuple<double, double>> _averageExecutionTime;

        private readonly List<Tuple<double, double>> _persentWaitingTime;
        private readonly List<Tuple<double, double>> _persentWorkingTime;

        private readonly List<Tuple<double, int>> _finishedTasks;
        private readonly List<Tuple<double, int>> _totalTasks;

        private readonly List<Task> _tasks;

        public TasksWorkStatistics(List<Task> tasks)
        {
            _averageProcessingTime = new List<Tuple<double, double>>();
            _avarageWatingTime = new List<Tuple<double, double>>();
            _averageExecutionTime = new List<Tuple<double, double>>();
            _persentWaitingTime = new List<Tuple<double, double>>();
            _persentWorkingTime = new List<Tuple<double, double>>();
            _finishedTasks = new List<Tuple<double, int>>();
            _totalTasks = new List<Tuple<double, int>>();

            _tasks = tasks;
        }

        public void Update(TaskFinishedEvent @event)
        {
            var task = @event.Task;

            var newProcessTime = task.EndTime - task.StartTime;
            var newWaitingTime = task.StartTime - task.CreatedTime;
            var newExecutionTime = task.EndTime- task.CreatedTime;
            var additionalSize = 1;

            AddAverageValue(_averageProcessingTime, additionalSize, newProcessTime);
            AddAverageValue(_avarageWatingTime, additionalSize, newWaitingTime);
            AddAverageValue(_averageExecutionTime, additionalSize, newExecutionTime);

            var additionalTime = newExecutionTime;

            AddAverageValue(_persentWaitingTime, additionalTime, newWaitingTime);
            AddAverageValue(_persentWorkingTime, additionalTime, newProcessTime);

            _finishedTasks.Add(new Tuple<double, int>(task.EndTime, _finishedTasks.Count + 1));
        }

        public void Update(TaskArrivalEvent @event)
        {
            var task = @event.Task;
            _totalTasks.Add(new Tuple<double, int>(task.CreatedTime, _totalTasks.Count + 1));
        }

        public void Update(WorkerArrivalEvent @event)
        {
            return;
        }

        public void Update(WorkerLeaveEvent @event)
        {
            return;
        }

        public double GetAvarageProcessingTime()
        {
            Log.D("Get Avarage Processing Time: (tasks, average) \n" + Print(_averageProcessingTime));
            return _averageProcessingTime.Last().Item2;
        }

        public double GetAvarageWaitingTime()
        {
            Log.D("Get Avarage Waiting Time: (tasks, average) \n" + Print(_avarageWatingTime));
            return _avarageWatingTime.Last().Item2;
        }

        public double GetAvarageExecutionTime()
        {
            Log.D("Get Avarage Execution Time: (tasks, average) \n" + Print(_averageExecutionTime));
            return _averageExecutionTime.Last().Item2;
        }

        public double GetParsentOfWaitTime()
        {
            Log.D("Get Parsent Wait Time: (total tasks exists time, wait time % / 100) \n" + Print(_persentWaitingTime));
            return _persentWaitingTime.Last().Item2;
        }

        public double GetParsentOfworkTime()
        {
            Log.D("Get Parsent Work Time: (total tasks exists time, wait time % / 100) \n" + Print(_persentWorkingTime));
            return _persentWorkingTime.Last().Item2;
        }

        public double GetFinishedTasks()
        {
            Log.D("Get Finished Tasks: (time, number)\n" + Print(_finishedTasks));
            return _finishedTasks.Last().Item2;
        }

        public double GetCreatedTasks()
        {
            Log.D("Get Created Tasks: (time, number)\n" + Print(_totalTasks));
            return _totalTasks.Last().Item2;
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

        public string Print<T, M>(List<Tuple<T, M>> workingSet)
        {
            var sb = new StringBuilder();

            foreach (var tuple in workingSet)
            {
                sb.AppendLine($"x: {tuple.Item1}, value {tuple.Item2, -7:#0.###}");
            }
            return sb.ToString();
        }

        private void AddAverageValue(List<Tuple<double, double>> workingSet, double additionalSize, double newValue)
        {
            

            if (workingSet.Count == 0)
            {
                if (newValue / additionalSize <= 0)
                    return;
            
                workingSet.Add(new Tuple<double, double>(additionalSize, newValue/ additionalSize));
                return;
            }

            var prevAverage = workingSet.Last()?.Item2 ?? 0;
            var prevNumberOfTasks = workingSet.Last()?.Item1 ?? 0;
            var average = LMath.Average(prevAverage, prevNumberOfTasks, newValue, additionalSize);
            workingSet.Add(new Tuple<double, double>(prevNumberOfTasks + additionalSize, average));
        }


    }
}