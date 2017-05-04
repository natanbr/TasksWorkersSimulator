using System;
using System.Linq;
using MathNet.Numerics.Distributions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskSimulation;
using TaskSimulation.ChooseAlgorithms;
using TaskSimulation.Distribution;
using TaskSimulation.Results;
using TaskSimulation.Simulator;

namespace TaskSimulationTests.Simulator
{
    [TestClass()]
    public class SimulateServerTests
    {
        int _initialNumOfWorkers = 10;
        double _maxSimulationTime = 10;

        [TestMethod()]
        public void SimulateServerTest()
        {
            _initialNumOfWorkers = 1;
            _maxSimulationTime = 5;

            DistFactory.TaskArrivalTime = new ContinuousUniform(1, 1);
            DistFactory.WorkerArrivalTime = new ContinuousUniform(100, 100); // never
            DistFactory.WorkerLeaveTime = new ContinuousUniform(100, 100); // never
            DistFactory.FeedbackDistribution = new ContinuousUniform(7, 7);
            DistFactory.QualityGrade = new ContinuousUniform(7, 7);
            DistFactory.ResponseTime = new ContinuousUniform(3, 3);
            DistFactory.GradeSystem = new OriginalGradeCalc();

            var executionSummary = SingleExecution();



        }

        public ExecutionSummary SingleExecution()
        {
            var simulator = new SimulateServer(_maxSimulationTime);

            simulator.Initialize(_initialNumOfWorkers);

            simulator.Start();

            Log.I();
            Log.I();
            Log.I();

            var executionStatistics = new ExecutionSummary()
            {
                ExecutionTime = _maxSimulationTime,

                TotalWorkersUtilization = simulator.Utilization.GetTotalWorkersUtilization(),
                TotalSystemUtilization = simulator.Utilization.GetSystemUtilization(),
                TotalTasksWait = simulator.Utilization.TaskWereInWaitList(),
                FinishedTasksForSingleExecution = simulator.Utilization.GetNumberOfFinishedTasks(),
                TotalTasksForSingleExecution = simulator.Utilization.GetNumberOfTotalTasks(),
            };

            return executionStatistics;
        }
    }
}