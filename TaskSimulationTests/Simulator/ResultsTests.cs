using System;
using MathNet.Numerics.Distributions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskSimulation;
using TaskSimulation.ChooseAlgorithms;
using TaskSimulation.Distribution;
using TaskSimulation.Results;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Workers;
using TaskSimulation.Utiles;

namespace TaskSimulationTests.Simulator
{
    [TestClass()]
    public class ResultsTests
    {
        int _initialNumOfWorkers = 10;
        double _maxSimulationTime = 10;

        private SimulateServer _simulator;

        [TestMethod()]
        public void SystemUtilizationTest()
        {
            _initialNumOfWorkers = 2;
            _maxSimulationTime = 10.5;

            GenerateShema();

            SimDistribution.I.GradeSystem = new OriginalGradeCalc();
            SimDistribution.I.GradeSystemChooseMethod = SimDistribution.I.GradeSystem.ChooseMethod();

            var r = SingleExecution();

            r.SystemUtilizationStatistics.AddLastValue();
            var sysUtil = r.SystemUtilizationStatistics.GetSystemUtilization();

            Assert.AreEqual(0.5,    sysUtil[0].Item2);
            Assert.AreEqual(1,      sysUtil[1].Item2);
            Assert.AreEqual(0.5,    sysUtil[2].Item2);
            Assert.AreEqual(1,      sysUtil[3].Item2);
            Assert.AreEqual(1,      sysUtil[4].Item2);
        }

        [TestMethod()]
        public void AvarageSystemUtilizationTest()
        {
            _initialNumOfWorkers = 2;
            _maxSimulationTime = 10.5;

            GenerateShema();

            SimDistribution.I.GradeSystem = new OriginalGradeCalc();
            SimDistribution.I.GradeSystemChooseMethod = SimDistribution.I.GradeSystem.ChooseMethod();

            var r = SingleExecution();

            r.SystemUtilizationStatistics.AddLastValue();
            var sysAvarageUtil = r.SystemUtilizationStatistics.GetAvarageSystemUtilization();

            Assert.AreEqual(0.5, sysAvarageUtil[0].Item2);
            Assert.AreEqual(0.5, sysAvarageUtil[1].Item2);

            Assert.IsTrue(sysAvarageUtil[2].Item2 < 0.92 && sysAvarageUtil[2].Item2 > 0.91);
            Assert.IsTrue(sysAvarageUtil[3].Item2 < 0.92 && sysAvarageUtil[3].Item2 > 0.91);
            Assert.IsTrue(sysAvarageUtil[4].Item2 < 0.96 && sysAvarageUtil[4].Item2 > 0.93);
        }

        [TestMethod()]
        public void AvarageWorkersEfficiencyTest()
        {
            _initialNumOfWorkers = 2;
            _maxSimulationTime = 10.5;

            GenerateShema();

            SimDistribution.I.GradeSystem = new OriginalGradeCalc();
            SimDistribution.I.GradeSystemChooseMethod = SimDistribution.I.GradeSystem.ChooseMethod();

            var r = SingleExecution();

            var txtRes = _simulator.GetShortOutput();

            var awf = r.WorkersStatistics.GetAvarageWorkersEfficiency();

            var avrg = (double)(0 + 1)/2;
            Assert.AreEqual(avrg, awf[1].Item2);
            avrg = ((avrg*2) + (3.0/4))/2;
            Assert.AreEqual(avrg, Math.Round(awf[2].Item2,3));
            Assert.AreEqual(avrg, Math.Round(awf[3].Item2,3));
            Assert.AreEqual(0.929, Math.Round(awf[4].Item2,3));


        }


        private void GenerateShema()
        {
            var never = _maxSimulationTime + 1;
            SimDistribution.I.Initialize(0);

            var shema = InputXmlShema.Default;
            var exec = shema.Executions[0];
            exec.TaskArrivalRate.Type = "ContinuousUniform";
            exec.TaskArrivalRate.Params = new double[] {1, 1};
            exec.WorkerArrivalRate.Type = "ContinuousUniform";
            exec.WorkerArrivalRate.Params = new double[] { never, never };
            exec.WorkerLeaveRate.Type = "ContinuousUniform";
            exec.WorkerLeaveRate.Params = new double[] { never, never };
            var wQuality = exec.WorkersQualityDistribution;
            wQuality.FeedbackMean.Type = "ContinuousUniform";
            wQuality.FeedbackMean.Params = new double[] {7, 7};
            wQuality.FeedbackStd.Type = "ContinuousUniform";
            wQuality.FeedbackStd.Params = new double[] {2, 2};
            wQuality.QualityMean.Type = "ContinuousUniform";
            wQuality.QualityMean.Params = new double[] {10, 10};
            wQuality.QualityStd.Type = "ContinuousUniform";
            wQuality.QualityStd.Params = new double[] {0.001, 0.001};
            wQuality.ProcessingTimeMean.Type = "ContinuousUniform";
            wQuality.ProcessingTimeMean.Params = new double[] {3, 3};
            wQuality.ProcessingTimeStd.Type = "ContinuousUniform";
            wQuality.ProcessingTimeStd.Params = new double[] { 0.000001, 0.000001 };
            SimDistribution.I.LoadData(0, shema);
        }


        public Utilization SingleExecution()
        {
            _simulator = new SimulateServer(_maxSimulationTime);

            _simulator.Initialize(_initialNumOfWorkers);

            _simulator.Start();

            Log.I();
            Log.I();
            Log.I();

            return _simulator.GetResults();
        } 
    }
}