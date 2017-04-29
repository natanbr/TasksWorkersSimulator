using System;
using MathNet.Numerics.Distributions;
using TaskSimulation.ChooseAlgorithms;
using TaskSimulation.Simulator;


namespace TaskSimulation.Distribution
{
    public static class DistFactory
    {
        public static ISimulatorDistribution TaskArrivalRate { get; set; }
        public static ISimulatorDistribution WorkerArrivalRate { get; set; }
        public static ISimulatorDistribution WorkerLeftRate { get; set; }
        public static ISimulatorDistribution TaskCompliteRate { get; set; }

        public static IContinuousDistribution FeedbackDistribution { get; set; }
        public static IContinuousDistribution QualityGrade { get; set; }
        //public static IContinuousDistribution ResponseGrade { get; set; }

        public static IGradeCalcAlgo GradeSystem { get; set; }

        public enum Distribution
        {
            Normal,
            Exponential,
            Uniform,
            Test,
        }
    }

    /// <summary>
    /// using lib: https://numerics.mathdotnet.com/Probability.html
    /// </summary>
    public class NormalDistTest : ISimulatorDistribution
    {
        private double _mean;
        private double _stddev;
        private double _test;
        private double _sample;
        /// <summary>
        /// Normal distribution wrapper
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="stddev"></param>
        /// <param name="test">test value for Test method</param>
        public NormalDistTest(double mean, double stddev, double test)
        {
            _mean = mean;
            _stddev = stddev;
            _test = test;
        }

        /// <summary>
        /// If test smaller or equals return true
        /// </summary>
        /// <returns></returns>
        public bool Test()
        {
            var normal = new Normal(_mean, _stddev);
            _sample = normal.Sample();
            return _test <= _sample;
        }

        public void PrintLastCalc(string text)
        {
            Log.I($"{text} - sample {_sample} >= test {_test}");
        }
    }

    public class TestDist : ISimulatorDistribution
    {
        private int _counter;
        private readonly int _mod;
        private readonly int _test;
        private string _text;

        public TestDist(int counter = 0, int mod = 2, int test = 0)
        {
            this._counter = counter;
            this._mod = mod;
            this._test = test;
        }

        public TestDist(string text, int counter = 0, int mod = 2, int test = 0)
        {
            this._counter = counter;
            this._mod = mod;
            this._test = test;
            this._text = text;
        }

        public void PrintLastCalc(string text)
        {
            Log.I("");
        }

        public bool Test()
        {
            //Log.I($"({SimulateServer.SimulationClock} + {_counter}) % {_mod} == {_test} {(SimulateServer.SimulationClock + _counter) % _mod == _test}                         {_text}");
            return ((SimulateServer.SimulationClock + _counter) % _mod == _test);
        }
    }

    public interface ISimulatorDistribution
    {
        bool Test();

        string ToString();

        void PrintLastCalc(string text);
    }
}
