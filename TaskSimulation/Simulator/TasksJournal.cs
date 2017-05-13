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

            task.OnTaskAssigned += _ =>
            {
                _unassignedTasks.Remove(task);
            };

            //Log.Event($"T> {task} Arrived");
        }

        public void Update(TaskFinishedEvent @event)
        {
            var task = @event.Task;

            _unassignedTasks.Remove(task);
            task.Finished();
        }

        public void Update(WorkerArrivalEvent @event)
        {
            if (_unassignedTasks.Count > 0)
            {
                Log.I("There are more tasks then workers..");
                @event.Worker?.Assign(_unassignedTasks[0]);
            }
        }

        public void Update(WorkerLeaveEvent @event)
        {
        }       
    }
}