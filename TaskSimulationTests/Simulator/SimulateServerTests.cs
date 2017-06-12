using System;
using MathNet.Numerics.Distributions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskSimulation;
using TaskSimulation.ChooseAlgorithms;
using TaskSimulation.Distribution;
using TaskSimulation.Results;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Workers;

namespace TaskSimulationTests.Simulator
{
    [TestClass()]
    public class SimulateServerTests
    {
        int _initialNumOfWorkers = 10;
        double _maxSimulationTime = 10;

        private PrivateObjectHelper _dist = new PrivateObjectHelper();

        // TODO verify results
        [TestMethod()]
        public void SimulateKnownParamsTest()
        {
            _initialNumOfWorkers = 1;
            _maxSimulationTime = 5;

            SimDistribution.I.Initialize(0);

            _dist.SetTaskArrivalRate(new ContinuousUniform(1, 1));
            _dist.SetWorkerArrivalRate(new ContinuousUniform(100, 100));
            _dist.SetWorkerLeaveRate(new ContinuousUniform(100, 100));
            
            _dist.SetWorkersQualityDistribution(new WorkersQualityDistribution()
            {
                FeedbackMean = new ContinuousUniform(1, 10),
                FeedbackStd = new ContinuousUniform(1, 2),
                QualityMean = new ContinuousUniform(1, 10),
                QualityStd = new ContinuousUniform(1, 2),
                ResponseMean = new ContinuousUniform(3,3),
                ResponseStd = new ContinuousUniform(0.01,0.01)
            });

            SimDistribution.I.GradeSystem = new OriginalGradeCalc();

            var executionSummary = SingleExecution();

            Assert.AreEqual(100, executionSummary.TotalWorkersUtilization *100); //%
            Assert.AreEqual(100, executionSummary.FinishedTasksForSingleExecution * 100);  //%
            Assert.AreEqual(Math.Round(4.98 / 8.98, 3), Math.Round(executionSummary.TotalTasksWait, 3));
            Assert.AreEqual(5, executionSummary.TotalTasksForSingleExecution);
        }

        [TestMethod()]
        public void SimulateWorkerLeaveTest()
        {
            _initialNumOfWorkers = 1;
            _maxSimulationTime = 100;

            _dist.SetTaskArrivalRate(new Exponential(0.34, SimDistribution.I.GlobalRandom));
            _dist.SetWorkerArrivalRate(new Normal(5, 3, SimDistribution.I.GlobalRandom));
            _dist.SetWorkerLeaveRate(new Exponential(0.8, SimDistribution.I.GlobalRandom));

            _dist.SetWorkersQualityDistribution(new WorkersQualityDistribution()
            {
                FeedbackMean = new ContinuousUniform(1, 10),
                FeedbackStd = new ContinuousUniform(1, 2),
                QualityMean = new ContinuousUniform(1, 10),
                QualityStd = new ContinuousUniform(1, 2),
                ResponseMean = new ContinuousUniform(3, 3),
                ResponseStd = new ContinuousUniform(0.1, 0.1)
            });

            SimDistribution.I.GradeSystem = new OriginalGradeCalc();
            var executionSummary = SingleExecution();
        }

        [TestMethod()]
        public void SimulateWithGlobalSeedTest()
        {
            _initialNumOfWorkers = 1;
            _maxSimulationTime = 10; // Mast me much larger then the initial pool size (10)
            SimDistribution.I.Initialize(1);

            _dist.SetTaskArrivalRate(new ContinuousUniform(1, 1, SimDistribution.I.GlobalRandom));
            _dist.SetWorkerArrivalRate(new ContinuousUniform(1, 1, SimDistribution.I.GlobalRandom));
            _dist.SetWorkerLeaveRate(new ContinuousUniform(1, 2, SimDistribution.I.GlobalRandom));

            _dist.SetWorkersQualityDistribution(new WorkersQualityDistribution()
            {
                FeedbackMean = new ContinuousUniform(7, 7, SimDistribution.I.GlobalRandom),
                FeedbackStd = new ContinuousUniform(0.0001, 0.0001, SimDistribution.I.GlobalRandom),
                QualityMean = new ContinuousUniform(3, 3, SimDistribution.I.GlobalRandom),
                QualityStd = new ContinuousUniform(0.0001, 0.0001, SimDistribution.I.GlobalRandom),
                ResponseMean = new ContinuousUniform(3, 3, SimDistribution.I.GlobalRandom),
                ResponseStd = new ContinuousUniform(0.1, 0.1, SimDistribution.I.GlobalRandom)
            });

            var executionSummary = SingleExecution();

            Assert.AreEqual((int)((0 / 23.5) * 100), (int)(executionSummary.TotalWorkersUtilization * 100), "TotalWorkersUtilization");
            Assert.AreEqual(0, executionSummary.FinishedTasksForSingleExecution, "FinishedTasksForSingleExecution");
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
            _dist.SetTaskArrivalRate(new ContinuousUniform(1, 5, SimDistribution.I.GlobalRandom));

            var r1 = SimDistribution.I.GlobalRandom.Next();
            var s1 = SimDistribution.I.TaskArrivalRate.Sample();
            var s2 = SimDistribution.I.TaskArrivalRate.Sample();
            var s3 = SimDistribution.I.TaskArrivalRate.Sample();
            var s4 = SimDistribution.I.TaskArrivalRate.Sample();

            SimDistribution.I.Initialize(Guid.NewGuid().GetHashCode());
            _dist.SetTaskArrivalRate(new ContinuousUniform(1, 5, SimDistribution.I.GlobalRandom));

            var r2 = SimDistribution.I.GlobalRandom.Next();
            var s5 = SimDistribution.I.TaskArrivalRate.Sample();
            var s6 = SimDistribution.I.TaskArrivalRate.Sample();
            var s7 = SimDistribution.I.TaskArrivalRate.Sample();
            var s8 = SimDistribution.I.TaskArrivalRate.Sample();

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
                TotalTasksWait = simulator.Utilization.TasksWorkStatistics.TaskWereInWaitList(),
                FinishedTasksForSingleExecution = simulator.Utilization.GetNumberOfFinishedTasks(),
                TotalTasksForSingleExecution = simulator.Utilization.GetNumberOfTotalTasks(),
            };

            return executionStatistics;
        } 
    }
}