using System;
using System.Diagnostics;
using System.Linq;
using TaskSimulation;
using TaskSimulation.ChooseAlgorithms;
using TaskSimulation.Distribution;
using TaskSimulation.Results;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Workers;
using TaskSimulation.Utiles;

namespace TaskSimulationCmd
{
    class Program
    {
        // Assumptions: 
        // - Task is assigned only to one worker
        // - Each worker has only one active task, may have more then one in the queue
        // - Always adding 1 worker event and 1 task event at time 0
        // - When worker leave, the task is NOT reassigned
        // - If worker leave during task execution the "BusyTime" is not updated
        // Questions:
        // - What to show in the graphs?
        // - What is the implementation of the Grade calculation?
        // - What are the parameters for the Distributions?
        // - What will happen to all the tasks in the workers queue when worker leave???
        // TODOs
        // - Add Personal seed and params for workers grade
        // - When Worker didn't complete the task and left -> End time = worker left

        static ExecutionSummary[] _summaries;
        static Stopwatch _stopwatch = new Stopwatch();

        static void Main(string[] args)
        {
            var execData = LoadInputFile(args);
            if (execData == null) return;

            SimDistribution.I.Initialize(Guid.NewGuid().GetHashCode());
            Log.I($"Global seed is {SimDistribution.I.GlobalSeed}");
            Log.I($"Global Random is {SimDistribution.I.GlobalRandom}");

            var executions = execData.Executions.Length;
            _summaries = new ExecutionSummary[executions];

            // TODO move grade system to file
            //SimDistribution.I.GradeSystem = new OriginalGradeCalc();
            SimDistribution.I.GradeSystem = new QueueLengthGradeCalc();
            SimDistribution.I.GradeSystemChooseMethod = SimDistribution.I.GradeSystem.ChooseMethod();
            for (var i = 0; i < executions; i++)
            {
                // Load the execution data for each iteration
                var loadStatus = SimDistribution.I.LoadData(i, execData);

                if (!loadStatus)
                {
                    Log.Err($"Load data for execution {i} failed.");
                    return;
                }

                var initialNumOfWorkers = execData.Executions[i].InitialNumOfWorkers;
                var maxSimulationTime   = execData.Executions[i].MaxSimulationTime;

                Log.I($"------------ Simulation Execution {i} ------------", ConsoleColor.DarkCyan);

                _stopwatch.Restart();
                _summaries[i] = SingleExecution(maxSimulationTime, initialNumOfWorkers);
                _stopwatch.Stop();

                Log.I($"Execution -{i}- Runtime: {_stopwatch.Elapsed}", ConsoleColor.Blue);
                Log.I();

            }

            Log.I();
            Log.I("----------- Print Results ----------- ", ConsoleColor.Blue);
            _summaries.ToList().ForEach(v => Log.I(v.ToString()));
        }

        public static ExecutionSummary SingleExecution(double time, long workers)
        {
            var simulator = new SimulateServer(time);

            simulator.Initialize(workers);

            simulator.Start();

            Log.I();
            Log.I();
            Log.I("----------- Post execution calculations ----------- ", ConsoleColor.Blue);

            var executionStatistics = new ExecutionSummary()
            {
                ExecutionTime = time,

                TotalWorkersUtilization = simulator.Utilization.GetTotalWorkersUtilization(),
                TotalSystemUtilization = simulator.Utilization.GetSystemUtilization(),
                FinishedTasksForSingleExecution = simulator.Utilization.GetNumberOfFinishedTasks(),
                TotalTasksForSingleExecution = simulator.Utilization.GetNumberOfTotalTasks(),
                TotalTasksWait = simulator.Utilization.TasksWorkStatistics.TaskWereInWaitList()
            };

            simulator.Utilization.TasksWorkStatistics.GetAvarageProcessingTime();
            simulator.Utilization.TasksWorkStatistics.GetAvarageWaitingTime();
            simulator.Utilization.TasksWorkStatistics.GetAvarageExecutionTime();
            simulator.Utilization.TasksWorkStatistics.GetParsentOfWaitTime();
            simulator.Utilization.TasksWorkStatistics.GetParsentOfworkTime();
            simulator.Utilization.TasksWorkStatistics.GetFinishedTasks();
            simulator.Utilization.TasksWorkStatistics.GetCreatedTasks();

            return executionStatistics;
        }

        private static InputXmlShema LoadInputFile(string[] args)
        {
            var inputFile = @"SimExe.xml";
            if (args?.Length > 0)
                inputFile = args[0];
            var execData = XmlUtils.Load<InputXmlShema>(inputFile);

            if (execData?.Executions == null || execData?.Executions?.Length <= 0)
            {
                Log.Err("File load failed, exitting execution.");
                XmlUtils.Save("", new InputXmlShema());
                return null;
            }

            return execData;
        }

        
    }

}
