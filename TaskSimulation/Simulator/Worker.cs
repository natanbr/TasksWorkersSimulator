using System;
using System.Collections.Generic;
using TaskSimulation.Distribution;
using TaskSimulation.Results;
using TaskSimulation.Simulator;
using TaskSimulation.Workers;

namespace TaskSimulation
{
    public class Worker
    {
        public static int WORKER_ID = 0;

        private int ID;
        private readonly List<Task> _queuedTasks;
        public bool WorkerStatus { get; private set; }
        public Grade Grade { get; set; }
        public WorkerUtilization Utilization;
        //public event Action<Worker> OnWorkerNotAvailable;

        public Worker()
        {
            Utilization = new WorkerUtilization();
            WorkerStatus = true;
            _queuedTasks = new List<Task>();
            ID = WORKER_ID++;

            // The F,Q,R... will be used for next calc, Grade will be used as initial grade 
            Grade = new Grade()
            {
                FeedbackGrade = DistFactory.GradeSystem.InitialGrade(),
                QualityGrade = DistFactory.GradeSystem.InitialGrade(),
                ResponseGrade = DistFactory.GradeSystem.InitialGrade(),
                NumberOfTasksGrade = DistFactory.GradeSystem.GetMaxNumberOfTasks(),
            };

            Grade.TotalGrade = DistFactory.GradeSystem.GetFinalGrade(Grade);
        }

        public void Assign(Task task)
        {
            AddTask(task);
        }

        private void AddTask(Task task)
        {
            // Add the task
            _queuedTasks.Add(task);
            task.Assign(this);
            Grade = DistFactory.GradeSystem.TaskAdded(Grade);
        }

        public void RemoveTask(Task task)
        {
            Log.Event($"{this} finished {task}, duration: {task.EndTime - task.StartTime}");
            _queuedTasks.Remove(task);

            Grade = DistFactory.GradeSystem.TaskRemoved(Grade, task.EndTime - task.StartTime);
            Grade = DistFactory.GradeSystem.GenerateRandomGrade(Grade);
        }

        // TODO move out from here
        private void UpdateStatistics()
        {
            if (_queuedTasks.Count == 0)
                Utilization.FreeTime++;
            else Utilization.BusyTime++;
        }

        public override string ToString()
        {
            return $"Worker: {ID} ";
        }

    }

    
}
