using System;
using System.Collections.Generic;
using TaskSimulation.Distribution;

namespace TaskSimulation.Simulator
{
    public class TasksJournal : ISimulatable
    {
        private List<Task> _waitForWorkers;
        private List<Task> _activeTasks;

        public event Action<Task> OnAvailableTask;

        public TasksJournal()
        {
            _waitForWorkers = new List<Task>();
            _activeTasks = new List<Task>();
        }

        public void Update()
        {
            // Generate new event (Strategy)
            var addTask = DistFactory.TaskArrivalRate.Test();

            if (!addTask) return;
            
            // Create task
            var task = new Task();
            _waitForWorkers.Add(task);

            Log.Event($"T> {task} Arrived");

            // Register to assigned tasks
            task.OnTaskAssigned += assignedTask =>
            {
                _waitForWorkers.Remove(assignedTask);
                _activeTasks.Add(assignedTask);
            };
            
            // Register clear task event
            task.OnTaskComplite += completedTask =>
            {
                //Log.I($"<T< {completedTask} from jurnal");
                _activeTasks.Remove(completedTask);
            };

            // Notify about available event
            NotifyAvailableEvent(task);
        }

        private void NotifyAvailableEvent(Task task)
        {
            OnAvailableTask?.Invoke(task);
        }

        public Task FindAvailableTask()
        {
            return _waitForWorkers?.Count > 0 ? _waitForWorkers?[0] : null;
        }
    }
}