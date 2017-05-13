using System;
using TaskSimulation.Results;
using TaskSimulation.Simulator.Events;

namespace TaskSimulation.Simulator
{
    public class SimulateServer
    {
        public static double SimulationClock { get; private set; } // TNOW

        private TasksJournal _tasksJournal;
        private WorkersJournal _workersJournal;
        public static double SimulatorMaxRunTime { get; private set; }
        private SimulationEventMan _simulationEvents;
        public Utilization Utilization { get; private set; }

        public SimulateServer(double maxSimulationTime = Int32.MaxValue)
        {
            _simulationEvents = new SimulationEventMan();
            SimulationClock = 0;
            SimulatorMaxRunTime = maxSimulationTime;
            
            Utilization = new Utilization();
        }

        public void Initialize(int initialNumOfWorkers)
        {
            Log.D("* * * * * * * Init * * * * * * *");
            Task.TASK_ID = 0;

            _simulationEvents.InitializeEvents(1, 1);

            _tasksJournal = new TasksJournal();

            _workersJournal = new WorkersJournal(initialNumOfWorkers);
            Utilization.AddWorkers(_workersJournal.ActiveWorkers);

            Log.D("* * * * * * * Init * * * * * * *");
        }

        public void Start()
        {
            var nextEvent = _simulationEvents.GetNextEvent();

            while (nextEvent.ArriveTime < SimulatorMaxRunTime)
            {
                // Update the simulation clock to the new event time
                SimulationClock = nextEvent.ArriveTime;
                Log.I();
                Log.Event( $"{nextEvent} at time {SimulationClock}");

                nextEvent.Accept(_workersJournal);
                nextEvent.Accept(_tasksJournal);
                nextEvent.Accept(Utilization);

                PrintSimulationState();

                nextEvent = _simulationEvents.GetNextEvent();
            }
        }

        public void PrintSimulationState()
        {
            Log.I($"* * * * * * * {SimulationClock:###0.##}/{SimulatorMaxRunTime:####0.##} * * * * * * *", ConsoleColor.Yellow);

            _workersJournal.ActiveWorkers.ForEach(w =>
            {
                Log.I($"{w} {w.Grade}");
            });
        }
 
    }
}

