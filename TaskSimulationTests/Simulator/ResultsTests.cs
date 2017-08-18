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
    public class ResultsHelper
    {
        static int _initialNumOfWorkers = 10;
        static double _maxSimulationTime = 10;

        private SimulateServer _simulator;

        public static void GenerateShema()
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