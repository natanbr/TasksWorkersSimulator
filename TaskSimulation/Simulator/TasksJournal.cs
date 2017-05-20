using System;
using System.Collections.Generic;
using TaskSimulation.Distribution;
using TaskSimulation.Simulator.Events;

namespace TaskSimulation.Simulator
{
    public class TasksJournal : ISimulatable
    {
        private List<Task> _unassignedTasks;

        public TasksJournal()
        {
            _unassignedTasks = new List<Task>();
        }

        public void Update(TaskArrivalEvent @event)
        {
            var task = @event.Task;

            task.CreatedTime = SimulateServer.SimulationClock;

            _unassignedTasks.Add(task);

            task.OnAddedToWorker += w =>
            {
                _unassignedTasks.Remove(task);
            };

            task.OnTaskAssigned += w =>
            {
                AddFinisheEvent(@event, task, w);
            };

        }

        public void Update(TaskFinishedEvent @event)
        {
            var task = @event.Task;
            var worker = @event.Worker;

            _unassignedTasks.Remove(task);

            if (worker.IsOnline())
                task.Finished();
        }

        public void Update(WorkerArrivalEvent @event)
        {
            if (_unassignedTasks.Count > 0)
            {
                if (@event.Worker == null)
                    Log.Err("worker can't be null");

                Log.D("There are unassigned tasks, assigned this task to arrived worker", ConsoleColor.Magenta);
                Log.Event($"Assign {_unassignedTasks[0]} to {@event.Worker}");
                @event.Worker?.Assign(_unassignedTasks[0]);
               
            }
        }

        public void Update(WorkerLeaveEvent @event)
        {
        }

        private static void AddFinisheEvent(TaskArrivalEvent @event, Task task, Worker worker)
        {
            // TODO calc using worker's data
            double finishIn = 0;
            do
            {
                finishIn = SimDistribution.I.ResponseTime.Sample();
            } while (finishIn < 0);

            var time = task.StartTime + finishIn;

            if (time > 0)
                @event.EventMan.AddEvent(new TaskFinishedEvent(task, worker, time));
            else
            {
                Log.Err("Neg time assign!@#$");
            }
        }
    }
}