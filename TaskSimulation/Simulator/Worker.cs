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
        private long ID;
        private readonly List<Task> _queuedTasks;
        public bool WorkerStatus { get; private set; }
        public Grade Grade { get; set; }
        public WorkerStatistics Statistics;
        private bool _working = false;
        //public event Action<Worker> OnWorkerNotAvailable;

        public Worker(long id)
        {
            Statistics = new WorkerStatistics();
            WorkerStatus = true;
            _queuedTasks = new List<Task>();
            ID = id;

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
            // Add the task
            _queuedTasks.Add(task);

            Grade = DistFactory.GradeSystem.UpdateOnTaskAdd(Grade);

            TryDoWork();
        }


        public void RemoveTask(Task task)
        {
            Log.Event($"{this} finished {task}, duration: {task.EndTime - task.StartTime}");
            _queuedTasks.Remove(task);
            _working = false;
            Statistics.BusyTime += task.EndTime - task.StartTime;

            Grade = DistFactory.GradeSystem.UpdateOnTaskRemoved(Grade, task.EndTime - task.StartTime);
            Grade = DistFactory.GradeSystem.GenerateRandomGrade(Grade);

            // Start work on the next task
            TryDoWork();
        }

        public override string ToString()
        {
            return $"Worker: {ID,-3:##}";
        }

        private void TryDoWork()
        {
            if (_working || _queuedTasks.Count == 0) return;

            _queuedTasks[0].AssignedBy(this);
            _working = true;
        }

    }

    
}
