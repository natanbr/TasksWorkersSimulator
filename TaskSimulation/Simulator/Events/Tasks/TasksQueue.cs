using System.Collections.Generic;

namespace TaskSimulation.Simulator.Tasks
{
    public class TasksQueue
    {
        private readonly List<Task> _queuedTasks;

        public TasksQueue()
        {
            _queuedTasks = new List<Task>();
        }

        public void Add(Task task)
        {
            _queuedTasks.Add(task);
        }

        public int Count()
        {
            return _queuedTasks.Count;
        }

        public void Remove(Task task)
        {
            _queuedTasks.Remove(task);
        }

        public Task GetFirst()
        {
            if (!HasAvailableTask())
                return null;

            return _queuedTasks[0];
        }

        public List<Task> ToList()
        {
            return _queuedTasks;
        }

        public bool HasAvailableTask()
        {
            return _queuedTasks == null || _queuedTasks.Count > 0;
        }
    }
}