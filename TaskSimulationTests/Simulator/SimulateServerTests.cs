using System;
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
        public void SimulateKnownParamsTest()
        {
            _initialNumOfWorkers = 1;
            _maxSimulationTime = 5;

            SimDistribution.I.TaskArrivalTime   = new ContinuousUniform(1, 1);
            SimDistribution.I.WorkerArrivalTime = new ContinuousUniform(100, 100); // never
            SimDistribution.I.WorkerLeaveTime   = new ContinuousUniform(100, 100); // never
            SimDistribution.I.FeedbackDistribution = new ContinuousUniform(7, 7);
            SimDistribution.I.QualityGrade = new ContinuousUniform(7, 7);
            SimDistribution.I.ResponseTime = new ContinuousUniform(3, 3);
            SimDistribution.I.GradeSystem = new OriginalGradeCalc();

            var executionSummary = SingleExecution();

            Assert.AreEqual((double)7 / 8, executionSummary.TotalWorkersUtilization);
            Assert.AreEqual(2, executionSummary.FinishedTasksForSingleExecution);
            Assert.AreEqual((double)2 / 9, executionSummary.TotalTasksWait);
            Assert.AreEqual(5, executionSummary.TotalTasksForSingleExecution);
        }

        [TestMethod()]
        public void SimulateWorkerLeaveTest()
        {
            _initialNumOfWorkers = 1;
            _maxSimulationTime = 100;

            SimDistribution.I.TaskArrivalTime = new Exponential(0.34, SimDistribution.I.GlobalRandom);
            SimDistribution.I.ResponseTime = new Normal(5, 3 , SimDistribution.I.GlobalRandom);
            SimDistribution.I.WorkerArrivalTime = new Exponential(0.8, SimDistribution.I.GlobalRandom);
            SimDistribution.I.WorkerLeaveTime = new Exponential(0.01, SimDistribution.I.GlobalRandom);
            SimDistribution.I.FeedbackDistribution = new ContinuousUniform(1, 10, SimDistribution.I.GlobalRandom);
            SimDistribution.I.QualityGrade = new ContinuousUniform(2, 10, SimDistribution.I.GlobalRandom);


            SimDistribution.I.GradeSystem = new OriginalGradeCalc();
            var executionSummary = SingleExecution();
        }

        [TestMethod()]
        public void SimulateWithGlobalSeedTest()
        {
            _initialNumOfWorkers = 1;
            _maxSimulationTime = 10; // Mast me much larger then the initial pool size (10)
            SimDistribution.I.Initialize(1);

            SimDistribution.I.TaskArrivalTime = new ContinuousUniform(1, 1, SimDistribution.I.GlobalRandom);
            SimDistribution.I.WorkerArrivalTime = new ContinuousUniform(1, 1, SimDistribution.I.GlobalRandom); 
            SimDistribution.I.WorkerLeaveTime = new ContinuousUniform(1, 2, SimDistribution.I.GlobalRandom); 
            SimDistribution.I.FeedbackDistribution = new ContinuousUniform(7, 7, SimDistribution.I.GlobalRandom);
            SimDistribution.I.QualityGrade = new ContinuousUniform(7, 7, SimDistribution.I.GlobalRandom);
            SimDistribution.I.ResponseTime = new ContinuousUniform(3, 3, SimDistribution.I.GlobalRandom);
            SimDistribution.I.GradeSystem = new OriginalGradeCalc();

            var executionSummary = SingleExecution();

            Assert.AreEqual((int)((9 / 23.5) * 100), (int)(executionSummary.TotalWorkersUtilization * 100), "TotalWorkersUtilization");
            Assert.AreEqual(3, executionSummary.FinishedTasksForSingleExecution, "FinishedTasksForSingleExecution");
            Assert.AreEqual(10, executionSummary.TotalTasksForSingleExecution, "TotalTasksForSingleExecution");
        }


        [TestMethod()]
        public void SeedUsageTest()
        {
            var randomGen = new Random(5);
            var distWithSeed = new ContinuousUniform(1, 10000, randomGen);
            Assert.AreEqual(3384, (int)distWithSeed.Sample());
            Assert.AreEqual(2844, (int)distWithSeed.Sample());
            Assert.AreEqual(2630, (int)distWithSeed.Sample());
        }

        [TestMethod()]
        public void DifferentSeedDifferentResultTest()
        {
            SimDistribution.I.Initialize(Guid.NewGuid().GetHashCode());
            SimDistribution.I.TaskArrivalTime = new ContinuousUniform(1, 5, SimDistribution.I.GlobalRandom);

            var r1 = SimDistribution.I.GlobalRandom.Next();
            var s1 = SimDistribution.I.TaskArrivalTime.Sample();
            var s2 = SimDistribution.I.TaskArrivalTime.Sample();
            var s3 = SimDistribution.I.TaskArrivalTime.Sample();
            var s4 = SimDistribution.I.TaskArrivalTime.Sample();

            SimDistribution.I.Initialize(Guid.NewGuid().GetHashCode());
            SimDistribution.I.TaskArrivalTime = new ContinuousUniform(1, 5, SimDistribution.I.GlobalRandom);

            var r2 = SimDistribution.I.GlobalRandom.Next();
            var s5 = SimDistribution.I.TaskArrivalTime.Sample();
            var s6 = SimDistribution.I.TaskArrivalTime.Sample();
            var s7 = SimDistribution.I.TaskArrivalTime.Sample();
            var s8 = SimDistribution.I.TaskArrivalTime.Sample();

            Assert.AreNotEqual(r1, r2);
            Assert.AreNotEqual(s1 + s2 + s3 + s4, s5 + s6 + s7 + s8);
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