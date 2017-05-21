using MathNet.Numerics.Distributions;

namespace TaskSimulation.Simulator.Workers
{
    public class WorkersQualityDistribution
    {
        public IContinuousDistribution FeedbackMean { get; set; }
        public IContinuousDistribution FeedbackStd { get; set; }

        public IContinuousDistribution QualityMean { get; set; }
        public IContinuousDistribution QualityStd { get; set; }

        public IContinuousDistribution ResponseMean { get; set; }
        public IContinuousDistribution ResponseStd { get; set; }

        public bool Validate()
        {
            return FeedbackMean != null && FeedbackStd != null &&
                   QualityMean != null && QualityStd != null &&
                   ResponseMean != null && ResponseStd != null;
        }

        public WorkerQualies GenerateQualies()
        {
            return new WorkerQualies(
                FeedbackMean.Sample(), FeedbackStd.Sample(), 
                QualityMean.Sample(), QualityStd.Sample(),
                ResponseMean.Sample(), ResponseStd.Sample());
        }
    }
}