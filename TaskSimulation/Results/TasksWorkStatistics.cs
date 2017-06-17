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
            _averageProcessingTime = new List<Tuple<double, double>> {new Tuple<double, double>(0, 0)};
            _avarageWatingTime = new List<Tuple<double, double>> { new Tuple<double, double>(0, 0) };
            _averageExecutionTime = new List<Tuple<double, double>> { new Tuple<double, double>(0, 0) };
            _persentWaitingTime = new List<Tuple<double, double>> { new Tuple<double, double>(0, 0) };
            _persentWorkingTime = new List<Tuple<double, double>> { new Tuple<double, double>(0, 0) };
            _finishedTasks = new List<Tuple<double, int>> { new Tuple<double, int>(0, 0) };
            _totalTasks = new List<Tuple<double, int>> { new Tuple<double, int>(0, 0) };

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

            _finishedTasks.Add(new Tuple<double, int>(task.EndTime, _finishedTasks.Count));
        }

        public void Update(TaskArrivalEvent @event)
        {
            var task = @event.Task;
            _totalTasks.Add(new Tuple<double, int>(task.CreatedTime, _totalTasks.Count));
        }

        public void Update(WorkerArrivalEvent @event)
        {
            return;
        }

        public void Update(WorkerLeaveEvent @event)
        {
            return;
        }

        public double GetLastAvarageProcessingTime()
        {
            Log.D("Get Avarage Processing Time: \n" + Print(_averageProcessingTime, "Task:", "Average:"));
            return _averageProcessingTime.Last().Item2;
        }

        public List<Tuple<double, double>> GetAvarageProcessingTime()
        {
            return _averageProcessingTime;
        }

        public double GetLastAvarageWaitingTime()
        {
            Log.D("Get Avarage Waiting Time: \n" + Print(_avarageWatingTime, "Task:", "Average:"));
            return _avarageWatingTime.Last().Item2;
        }

        public List<Tuple<double, double>> GetAvarageWaitingTime()
        {
            return _avarageWatingTime;
        }

        public double GetLastAvarageExecutionTime()
        {
            Log.D("Get Avarage Execution Time (Wait + Process): \n" + Print(_averageExecutionTime, "Task:", "Average:"));
            
            return GetAvarageExecutionTime().Last().Item2;
        }

        public List<Tuple<double, double>> GetAvarageExecutionTime()
        {
            return _averageExecutionTime;
        }

        public double GetParsentOfWaitTime()
        {
            Log.D("Get Parsent Wait Time: \n" + Print(_persentWaitingTime, "Sum task exists:", "Wait time:"));
            return _persentWaitingTime.Last().Item2;
        }

        public double GetParsentOfworkTime()
        {
            Log.D("Get Parsent Work Time: \n" + Print(_persentWorkingTime, "Sum task exists:", "Work time:"));
            return _persentWorkingTime.Last().Item2;
        }

        public int GetFinishedTasks()
        {
            Log.D("Get Finished Tasks: \n" + Print(_finishedTasks, "Time:", "# of tasks:"));
            return _finishedTasks.Last().Item2;
        }

        public int GetCreatedTasks()
        {
            Log.D("Get Created Tasks: (time, number)\n" + Print(_totalTasks, "Time:", "# of tasks:"));
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

        public string Print<T, M>(List<Tuple<T, M>> workingSet, string xTitle = "x:", string yTitle = "y:")
        {
            var sb = new StringBuilder();

            foreach (var tuple in workingSet)
            {
                sb.AppendLine($"{xTitle} {tuple.Item1, -20}, {yTitle} {tuple.Item2, -7:#0.###}");
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

            var prevAverage = workingSet.Last().Item2 ;
            var prevNumberOfTasks = workingSet.Last().Item1;
            var average = LMath.AverageIncrementalSize(prevAverage, prevNumberOfTasks, newValue, additionalSize);
            workingSet.Add(new Tuple<double, double>(prevNumberOfTasks + additionalSize, average));
        }


    }
}