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

        /*public static DistributionType FeedbackDistributionType { get; set; }
        public static DistributionType QualityGradeType { get; set; }
        public static DistributionType ResponseTimeType { get; set; }

        public class GradeFactory
        {
            private Random _rand;

            public GradeFactory()
            {
                var seed = GlobalRandom.Next();
                _rand = new Random(seed);
            }

            public IContinuousDistribution GetContinuousDistribution(Type type, params double[] param)
            {
                return (IContinuousDistribution)Activator.CreateInstance(type, param);
            }
        }

        public class GradesDist
        {
            private int _seed;
            private Type _iContinuousType;

            public GradesDist(Type iContinuousType, int seed)
            {
                _seed = seed;
                _iContinuousType = iContinuousType;
            }

            public IContinuousDistribution FeedbackDistribution { get; set; }
            public IContinuousDistribution QualityGrade { get; set; }
            public IContinuousDistribution ResponseTime { get; set; }
        }

             public enum DistributionType
        {
            Normal,
            Exponential,
            Uniform,
            Test,
        }

    */
        public IGradeCalcAlgo GradeSystem { get; set; }

       
    }
}
