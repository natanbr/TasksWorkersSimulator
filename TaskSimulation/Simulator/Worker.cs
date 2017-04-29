using System;
using System.Collections.Generic;
using TaskSimulation.Distribution;
using TaskSimulation.Results;
using TaskSimulation.Simulator;
using TaskSimulation.Workers;

namespace TaskSimulation
{
    public class Worker :ISimulatable, IStatisticCollectorVisitor
    {
        public static int WORKER_ID = 0;

        private int ID;
        private readonly List<Task> _queuedTasks;
        public bool WorkerStatus { get; private set; }
        public Grade Grade { get; set; }
        public WorkerUtilization Utilization;
        public event Action<Worker> OnWorkerNotAvailable;

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
                TotalGrade = DistFactory.GradeSystem.InitialGrade(),
            };
        }

        public void Assign(Task task)
        {
            AddTask(task);

            // Remove it when finished its execution
            task.OnTaskComplite += RemoveTask;
        }

        private void AddTask(Task task)
        {
            // Add the task
            _queuedTasks.Add(task);
            task.Assign(this);
            Grade = DistFactory.GradeSystem.TaskAdded(Grade, _queuedTasks.Count);
        }

        private void RemoveTask(Task task)
        {
            Log.Event($"{this} finished {task}, duration: {task.EndTime - task.StartTime}");
            _queuedTasks.Remove(task);

            //Grade = DistFactory.GradeSystem.TaskRemoved(Grade);
            Grade = DistFactory.GradeSystem.TaskRemoved(Grade, _queuedTasks.Count, task.EndTime - task.StartTime);
            Grade = DistFactory.GradeSystem.GenerateRandomGrade(Grade);
        }

        /// <summary>
        /// todo talk if the order of comiltte and leave is important
        /// </summary>
        public void Update()
        {
            // Generate is work finished 
            var isWorkerLeft = DistFactory.WorkerLeftRate.Test();

            if (isWorkerLeft)
            {
                // DistFactory.WorkerLeftRate.PrintLastCalc("Worker left");
                Log.Event($"W< {this} left");
                WorkerStatus = false;
                OnWorkerNotAvailable?.Invoke(this);
                return;
            }

            // Update tasks
            UpdateTasks();

            UpdateStatistics();
        }

        // TODO move out from here
        private void UpdateStatistics()
        {
            if (_queuedTasks.Count == 0)
                Utilization.FreeTime++;
            else Utilization.BusyTime++;
        }

        private void UpdateTasks()
        {
            if (_queuedTasks.Count <= 0)
                return;

            // Assumption: worker works only on one task at a time.
            var nextTask = _queuedTasks[0];
            nextTask.Update();
        }

        public override string ToString()
        {
            return $"Worker: {ID} ";
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    
}
