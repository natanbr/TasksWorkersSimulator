using System;
using TaskSimulation.Results;
using TaskSimulation.Simulator.Events;
using TaskSimulation.Simulator.Tasks;
using TaskSimulation.Simulator.Workers;

namespace TaskSimulation.Simulator
{
    public class SimulateServer
    {
        public static double SimulationClock { get; private set; } // TNOW
        public static double SimulatorMaxRunTime { get; private set; }


        private readonly TasksJournal _tasksJournal;
        private readonly WorkersJournal _workersJournal;
        private readonly SimulationEventMan _simulationEvents;
        private readonly Utilization _utilization;// { get; private set; }
        private readonly DebugSimpleOutput _dbPrint;

        public SimulateServer(double maxSimulationTime = Int32.MaxValue)
        {
            _simulationEvents = new SimulationEventMan();
            SimulationClock = 0;
            SimulatorMaxRunTime = maxSimulationTime;
            
            _utilization = new Utilization();
            _tasksJournal = new TasksJournal();
            _workersJournal = new WorkersJournal();
            _dbPrint = new DebugSimpleOutput();
        }

        public void Initialize(long initialNumOfWorkers)
        {
            Log.D("* * * * * * * Init * * * * * * *");
            Task.TASK_ID = 0;

            _simulationEvents.InitializeGenesisEvents(1, initialNumOfWorkers);

            Log.D("* * * * * * * Init * * * * * * *");
        }

        public void Start()
        {
            var nextEvent = _simulationEvents.GetNextEvent();

            while (nextEvent.ArriveTime < SimulatorMaxRunTime)
            {
                // Update the simulation clock to the new event time
                if (nextEvent.ArriveTime < 0)
                    Log.Err("Time is < 0");

                SimulationClock = nextEvent.ArriveTime;
                Log.I();
                Log.Event( $"{nextEvent} at time {SimulationClock,-5:#0.##}");

                if (nextEvent is TaskArrivalEvent || nextEvent is TaskFinishedEvent)
                {
                    nextEvent.Accept(_tasksJournal);
                    nextEvent.Accept(_workersJournal);
                }
                else
                {
                    nextEvent.Accept(_workersJournal);
                    nextEvent.Accept(_tasksJournal);
                }

                nextEvent.Accept(_utilization);
                nextEvent.Accept(_dbPrint);

                #if DEBUG
                PrintSimulationState();
                #endif

                nextEvent = _simulationEvents.GetNextEvent();

            }

            //Log.D(_dbPrint.ToString());

            //PrintSimulationState();
        }

        public Utilization GetResults()
        {
            return _utilization;
        }

        public string GetShortOutput()
        {
            return _dbPrint.ToString();
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

