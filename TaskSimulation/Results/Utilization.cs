using System.Collections.Generic;
using System.Linq;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Events;
using TaskSimulation.Simulator.Tasks;
using TaskSimulation.Simulator.Workers;

//using Task = TaskSimulation.Simulator.Task;

namespace TaskSimulation.Results
{
    public class Utilization : ISimulatable
    {
        private readonly List<Task> _tasks;
        //private readonly List<Worker> _workers;
        private readonly Dictionary<Worker, WorkerData> _workers;


        public TasksWorkStatistics TasksWorkStatistics { get; set; }
        public WorkersStatistics WorkersStatistics { get; set; }

        public SystemUtilizationStatistics SystemUtilizationStatistics { get; set; }

        public Utilization()
        {
            _tasks = new List<Task>();
            _workers = new  Dictionary<Worker, WorkerData>();//List<Worker>();

            TasksWorkStatistics = new TasksWorkStatistics(_tasks);
            WorkersStatistics = new WorkersStatistics(_workers);
            SystemUtilizationStatistics = new SystemUtilizationStatistics();
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
            (@event as AEvent)?.Accept(SystemUtilizationStatistics);
        }








        /************ OLD CODE ***************/ // TODO move this to classes
/*        private double GetWorkersWorkedTime()
        {
            var sumWorkersWorkedTime = _workers.Sum(w =>
            {
                // The last not finished task
                var lastTask = (w.IsWorking && w.IsOnline()) ? SimulateServer.SimulationClock - w.GetCurrentTask().StartTime : 0;

                return w.Statistics.BusyTime + lastTask;
            });

            return sumWorkersWorkedTime;
        }

        public double GetTotalWorkersUtilization()
        {
            var sumWorkersBusy = GetWorkersWorkedTime();
            Log.D($"(S=Start time, E=End time, B=Busy time, T=Total existing time");
            var workersTotalTime = _workers.Sum(w =>
            {
                Log.D($"{w} (S:{w.Statistics.StartAt,-6:#0.###}, E:{w.Statistics.EndAt,-6:#0.###}) B:{w.Statistics.BusyTime,-6:#0.###} T:{w.Statistics.TotalTime,-6:#0.###}");
                return w.Statistics.TotalTime;
            });

            var workerUtilization = sumWorkersBusy / workersTotalTime;
            Log.I($"Workers utilization is (sum workers work time)/(sum workers work time)= workerUtilization");
            Log.I($"Workers utilization is {sumWorkersBusy}/{workersTotalTime} = {workerUtilization*100:N2}%");
            return workerUtilization;
        }*/

        /// <summary>
        /// (Total time the workers worked/Number of workers)/Simulation Run time = Utilization
        /// If all the workers were working all the time, the system utilization has been 100%
        /// </summary>
        /// <returns></returns>
       /* public double GetSystemUtilization()
        {
            var sumWorkersWorkedTime = GetWorkersWorkedTime();

            var systemUtilization = (sumWorkersWorkedTime /_workers.Count)/SimulateServer.SimulationClock;

            Log.I($"System utilization is: (sum Workers Worked Time/#workers)/Simulation Final Time");
            Log.I($"System utilization is: " +
                  $"({sumWorkersWorkedTime}/{_workers.Count})/{SimulateServer.SimulationClock} = {systemUtilization * 100:N2}%");
            return systemUtilization;
        }*/

        public class WorkerData
        {
            public double AvarageEfficiency { get; set; } = 0;
            public int NumberOfTasksFinished { get; set; } = 0;
        }
    }
}
