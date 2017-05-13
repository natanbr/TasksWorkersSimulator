using System;
using System.Linq;
using MathNet.Numerics.Distributions;
using TaskSimulation;
using TaskSimulation.ChooseAlgorithms;
using TaskSimulation.Distribution;
using TaskSimulation.Results;
using TaskSimulation.Simulator;

namespace TaskSimulationCmd
{
    class Program
    {
        // Assumptions: 
        // - Task is assigned only to one worker
        // - Each worker has only one active task, may have more then one in the queue
        // - Always adding 1 worker event and 1 task event at time 0
        // - When worker leave, the task is NOT reassigned
        // Questions:
        // - What to show in the graphs?
        // - What is the implementation of the Grade calculation?
        // - What are the parameters for the Distributions?
        // TODOs
        // - SimulateServerTest2 returns unrealistic results
        // - Add Personal seed and params for workers grade
        // - When Worker didn't complete the task and left -> End time = worker left

        const int NUM_OF_EXECUTIONS = 1;
        static readonly ExecutionSummary[] _summaries = new ExecutionSummary[NUM_OF_EXECUTIONS];

        const int INITIAL_NUM_OF_WORKERS = 1;
        const double MAX_SIMULATION_TIME = 10;

        static void Main()
        {
            SimDistribution.I.Initialize(Guid.NewGuid().GetHashCode());

            Log.I($"Global seed is {SimDistribution.I.GlobalSeed}");
            Log.I($"Global Random is {SimDistribution.I.GlobalRandom}");

            SimDistribution.I.TaskArrivalTime = new ContinuousUniform(1, 1, SimDistribution.I.GlobalRandom);
            SimDistribution.I.WorkerArrivalTime = new ContinuousUniform(1, 2, SimDistribution.I.GlobalRandom);
            SimDistribution.I.WorkerLeaveTime = new ContinuousUniform(1, 2, SimDistribution.I.GlobalRandom);
            SimDistribution.I.FeedbackDistribution = new ContinuousUniform(1, 10, SimDistribution.I.GlobalRandom);
            SimDistribution.I.QualityGrade = new ContinuousUniform(2, 10, SimDistribution.I.GlobalRandom);
            SimDistribution.I.ResponseTime = new ContinuousUniform(2, 4, SimDistribution.I.GlobalRandom);

            SimDistribution.I.GradeSystem = new OriginalGradeCalc();

            for (int i = 0; i < NUM_OF_EXECUTIONS; i++)
            {
                Log.I($"---------------------------- Simulation Execution {i} ----------------------------",
                    ConsoleColor.DarkCyan);
                _summaries[i] = SingleExecution();
            }

            Log.I();
            Log.I("----------- Print Results ----------- ", ConsoleColor.Blue);
            _summaries.ToList().ForEach(v => Log.I(v.ToString()));
        }

        public static ExecutionSummary SingleExecution()
        {
            var simulator = new SimulateServer(MAX_SIMULATION_TIME);

            simulator.Initialize(INITIAL_NUM_OF_WORKERS);

            simulator.Start();

            Log.I();
            Log.I();
            Log.I("----------- Post execution calculations ----------- ", ConsoleColor.Blue);

            var executionStatistics = new ExecutionSummary()
            {
                ExecutionTime = MAX_SIMULATION_TIME,

                TotalWorkersUtilization = simulator.Utilization.GetTotalWorkersUtilization(),
                TotalSystemUtilization = simulator.Utilization.GetSystemUtilization(),
                FinishedTasksForSingleExecution = simulator.Utilization.GetNumberOfFinishedTasks(),
                TotalTasksForSingleExecution = simulator.Utilization.GetNumberOfTotalTasks(),
                TotalTasksWait = simulator.Utilization.TaskWereInWaitList()
            };

            return executionStatistics;
        }
    }

}
