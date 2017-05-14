using System;
using MathNet.Numerics.Distributions;
using TaskSimulation.Distribution;

namespace TaskSimulation.Simulator.Workers
{
    public class WorkerDistribution
    {
        public IContinuousDistribution Feedback { get; }

        public IContinuousDistribution JobQuality { get; }

        public IContinuousDistribution ResponseTime { get; }

        public WorkerDistribution(WorkerQualies qualies)
        {
            // Generate private random for worker
            var privateRandom = new Random(SimDistribution.I.GlobalRandom.Next());

            //TODO get type from input file
            Feedback        = new Normal(qualies.FeedbackMean, qualies.FeedbackStd, privateRandom);
            JobQuality      = new Normal(qualies.QualityMean,  qualies.QualityStd,  privateRandom);
            ResponseTime    = new Normal(qualies.ResponseMean,  qualies.ResponseStd,  privateRandom);
        }

        public WorkerDistribution(IContinuousDistribution feedback, IContinuousDistribution jobQuality, IContinuousDistribution responseTime)
        {
            Feedback = feedback;
            JobQuality = jobQuality;
            ResponseTime = responseTime;
        }

    }
}