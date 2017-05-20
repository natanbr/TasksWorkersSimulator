using System;
using MathNet.Numerics.Distributions;
using TaskSimulation.ChooseAlgorithms;


namespace TaskSimulation.Distribution
{
    public class SimDistribution
    {
        private static SimDistribution _instance;
        public static SimDistribution Instance { get { return _instance = _instance ?? new SimDistribution(); } }
        public static SimDistribution I => Instance;
        private SimDistribution() { }

        public int GlobalSeed { get; private set; }
        public Random GlobalRandom { get; private set; }

        public void Initialize(int globalSeed)
        {
            GlobalSeed = globalSeed;
            GlobalRandom = new Random(GlobalSeed);
        }

        public IContinuousDistribution TaskArrivalTime { get; set; }

        public IContinuousDistribution WorkerArrivalTime { get; set; }
        public IContinuousDistribution WorkerLeaveTime { get; set; }

        public IContinuousDistribution FeedbackDistribution { get; set; }
        public IContinuousDistribution QualityGrade { get; set; }
        public IContinuousDistribution ResponseTime { get; set; }

        public IGradeCalcAlgo GradeSystem { get; set; }

       
    }
}
