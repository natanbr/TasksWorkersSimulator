using System.Collections.Generic;
using System.Linq;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Events;
using TaskSimulation.Simulator.Tasks;
using TaskSimulation.Simulator.Workers;

namespace TaskSimulation.Results
{
    public class Utilization : ISimulatable
    {
        private readonly List<Task> _tasks;
        //private readonly List<Worker> _workers;
        private readonly Dictionary<Worker, WorkerData> _workers;

        public TasksWorkStatistics TasksWorkStatistics { get; set; }
        public WorkersStatistics WorkersStatistics { get; set; }
        public SystemUtilizationStatistics2 SystemUtilizationStatistics2 { get; set; }
        public WorkersStatisticsGrouping WorkerStatisticsGrouping { get; set; }


        public Utilization()
        {
            _tasks = new List<Task>();
            _workers = new  Dictionary<Worker, WorkerData>();//List<Worker>();

            TasksWorkStatistics = new TasksWorkStatistics(_tasks);
            WorkersStatistics = new WorkersStatistics(_workers);
            SystemUtilizationStatistics2 = new SystemUtilizationStatistics2();
            WorkerStatisticsGrouping = new WorkersStatisticsGrouping();
        }

        public void Update(TaskArrivalEvent @event)
        {
            _tasks.Add(@event.Task);
            UpdateSubscribers(@event);
        }

        public void Update(TaskFinishedEvent @event)
        {
            UpdateSubscribers(@event);
        }

        public void Update(WorkerArrivalEvent @event)
        {
            _workers.Add(@event.Worker, new WorkerData());
            UpdateSubscribers(@event);
        }

        public void Update(WorkerLeaveEvent @event)
        {
            UpdateSubscribers(@event);
        }

        private void UpdateSubscribers<T>(T @event)
        {
            (@event as AEvent)?.Accept(TasksWorkStatistics);
            (@event as AEvent)?.Accept(WorkersStatistics);
            (@event as AEvent)?.Accept(SystemUtilizationStatistics2);
            (@event as AEvent)?.Accept(WorkerStatisticsGrouping);
        }




        
        
    }
}
