using System;
using TaskSimulation.Results;

namespace TaskSimulation.Simulator
{
    public class SimulateServer
    {
        public static long SimulationClock { get; private set; } // TNOW

        private TasksJournal _tasksJournal;
        private WorkersJournal _workersJournal;
        public static int SimulatorMaxRunTime { get; private set; }

        public Utilization Utilization { get; private set; }

        public SimulateServer(int maxSimulationTime = Int32.MaxValue)
        {
            SimulationClock = 0;
            SimulatorMaxRunTime = maxSimulationTime;
            
            Utilization = new Utilization();
        }

        public void Initialize(int initialNumOfWorkers)
        {
            Task.TASK_ID = 0;
            Worker.WORKER_ID = 0;
            _tasksJournal = new TasksJournal();
            _workersJournal = new WorkersJournal(initialNumOfWorkers);
        }

        public void Start()
        {
            _tasksJournal.OnAvailableTask += task =>
            {
                // Add statistics collector to task
                task.Accept(Utilization);

                //Log.D($"Notify task available ({task})");
                _workersJournal.AssignTask(task);

                task.OnTaskComplite += _ =>
                {
                    // remove the task from the workes
                    // update the workers grades
                };

            };

            _workersJournal.OnNewWorkerArrived += worker =>
            {
                // Add statistics collector to worker
                worker.Accept(Utilization);

                // In case there are more tasks them workers assign 
                var task = _tasksJournal.FindAvailableTask();
                if (task != null) worker.Assign(task);
            };

            for (;SimulationClock < SimulatorMaxRunTime; SimulationClock++)
            {
                PrintSimulationState();

                // Task update first to avoid false worker idle between creation and receiving task
                _tasksJournal.Update();
                _workersJournal.Update(); 
            }

        }

        public void PrintSimulationState()
        {
            Log.I();
            Log.I($"*** {SimulationClock:000}/{SimulatorMaxRunTime:000} ***");

            _workersJournal.ActiveWorkers.ForEach(w =>
            {
                Log.I($"{w} {w.Grade}");
            });
        }
 
    }
}

