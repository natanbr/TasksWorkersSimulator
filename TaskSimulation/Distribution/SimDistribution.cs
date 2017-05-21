using System;
using MathNet.Numerics.Distributions;
using TaskSimulation.ChooseAlgorithms;
using TaskSimulation.Simulator.Workers;
using TaskSimulation.Utiles;


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

        public void LoadData(int execution, InputXmlShema shema)
        {
            var execData = shema.Executions[execution];
            TaskArrivalRate = ReflectIContinuousDistribution.GetDistribution(execData.TaskArrivalRate.Type, execData.TaskArrivalRate.Params, GlobalRandom);
            WorkerArrivalRate = ReflectIContinuousDistribution.GetDistribution(execData.WorkerArrivalRate.Type, execData.TaskArrivalRate.Params, GlobalRandom);
            WorkerLeaveRate = ReflectIContinuousDistribution.GetDistribution(execData.WorkerLeaveRate.Type, execData.TaskArrivalRate.Params, GlobalRandom);

            var wqd = execData.WorkersQualityDistribution;
            WorkersQualityDistribution = new WorkersQualityDistribution()
            {
                FeedbackMean = ReflectIContinuousDistribution.GetDistribution(wqd.FeedbackMean.Type,    wqd.FeedbackMean.Params,    GlobalRandom),
                FeedbackStd = ReflectIContinuousDistribution.GetDistribution (wqd.FeedbackStd.Type,     wqd.FeedbackStd.Params,     GlobalRandom),
                QualityMean = ReflectIContinuousDistribution.GetDistribution (wqd.QualityMean.Type,     wqd.QualityMean.Params,     GlobalRandom),
                QualityStd = ReflectIContinuousDistribution.GetDistribution  (wqd.QualityStd.Type,      wqd.QualityStd.Params,      GlobalRandom),
                ResponseMean = ReflectIContinuousDistribution.GetDistribution(wqd.ResponseTimeMean.Type,wqd.ResponseTimeMean.Params,GlobalRandom),
                ResponseStd = ReflectIContinuousDistribution.GetDistribution (wqd.ResponseTimeStd.Type, wqd.ResponseTimeStd.Params, GlobalRandom),
            };

            var legal = WorkersQualityDistribution.Validate();
        }

        public IContinuousDistribution TaskArrivalRate { get; private set; }
        public IContinuousDistribution WorkerArrivalRate { get; private set; }
        public IContinuousDistribution WorkerLeaveRate { get; private set; }
        //public IContinuousDistribution FeedbackDistribution { get; private set; }
        //public IContinuousDistribution QualityGrade { get; private set; }
        //public IContinuousDistribution ResponseTime { get; private set; }

        public IGradeCalcAlgo GradeSystem { get; set; }

        public WorkersQualityDistribution WorkersQualityDistribution { get; private set; }
       
    }
}
