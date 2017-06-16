using TaskSimulation.Distribution;
using TaskSimulation.Results;
using TaskSimulation.Simulator.Tasks;

namespace TaskSimulation.Simulator.Workers
{
    public class Worker
    {
        private long ID;
        private readonly TasksQueue _tasks;
        public Grade Grade { get; set; }
        public WorkerExecData Statistics { get; set; }
        public WorkerDistribution Distribution { get; set; }
        public bool IsWorking { get; private set; } = false;

        public Worker(long id, WorkerQualies qualies)
        {
            ID = id;
            _tasks = new TasksQueue();
            Statistics = new WorkerExecData();
            Distribution = new WorkerDistribution(qualies);
            Grade = SimDistribution.I.GradeSystem.InitialGrade();
        }

        public void AddTask(Task task)
        {
            // Add the task
            _tasks.Add(task);

            task.SetStateAddedTo(this);

            Grade = SimDistribution.I.GradeSystem.UpdateOnTaskAdd(Grade);

            ContinueToNextTask();
        }


        public void RemoveTask(Task task)
        {
            var time = SimulateServer.SimulationClock;

            Log.Event($"{this} finished {task}, duration: {time - task.StartTime,-5:##.##}");
            _tasks.Remove(task);
            IsWorking = false;

            Statistics.UpdateWorkedTime(task);

            Grade = SimDistribution.I.GradeSystem.UpdateOnTaskRemoved(this, task);

            // Start work on the next task
            ContinueToNextTask();
        }

        public override string ToString()
        {
            return $"Worker: {ID,-3:##}";
        }

        private void ContinueToNextTask()
        {
            if (IsWorking || !_tasks.HasAvailableTask()) return;

            var nextTask = _tasks.GetFirst();

            Log.I($"{this} starting work on {nextTask}");

            nextTask.SetStateAssignedBy(this);
            IsWorking = true;
        }

        public Task GetCurrentTask()
        {
            return _tasks.GetFirst();
        }

        public bool IsOnline()
        {
            return Statistics.EndAt == -1;
        }
    }
}
