namespace TaskSimulation.Simulator.Workers
{
    public class WorkerQualies
    {
        public double FeedbackMean { get; }
        public double FeedbackStd { get; }
        public double QualityMean { get; }
        public double QualityStd { get; }
        public double ResponseMean { get; }
        public double ResponseStd { get; }

        public WorkerQualies(double feedbackMean, double feedbackStd, double qualityMean, double qualityStd, double responseMean, double responseStd)
        {
            FeedbackMean = feedbackMean;
            FeedbackStd = feedbackStd;
            QualityMean = qualityMean;
            QualityStd = qualityStd;
            ResponseMean = responseMean;
            ResponseStd = responseStd;
        }

        public bool Validate()
        {
            return (QualityMean > 0 && QualityStd > 0 && FeedbackStd > 0 && FeedbackMean > 0 && ResponseStd > 0 && ResponseMean > 0);
        }
    }
}