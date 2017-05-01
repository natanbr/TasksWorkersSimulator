using System;
using MathNet.Numerics.Distributions;
using TaskSimulation.ChooseAlgorithms;
using TaskSimulation.Simulator;


namespace TaskSimulation.Distribution
{
    public static class DistFactory
    {

        public static IContinuousDistribution TaskArrivalTime { get; set; }
        public static IContinuousDistribution WorkerArrivalTime { get; set; }

        public static IContinuousDistribution FeedbackDistribution { get; set; }
        public static IContinuousDistribution QualityGrade { get; set; }
        public static IContinuousDistribution ResponseTime { get; set; }

        public static IGradeCalcAlgo GradeSystem { get; set; }

        public enum Distribution
        {
            Normal,
            Exponential,
            Uniform,
            Test,
        }

    }


}
