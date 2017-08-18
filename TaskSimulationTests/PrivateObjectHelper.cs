using MathNet.Numerics.Distributions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskSimulation.Distribution;
using TaskSimulation.Simulator.Workers;

namespace TaskSimulationTests.Simulator
{
    public class PrivateObjectHelper
    {
        private PrivateObject _simDistribution;

        public PrivateObjectHelper()
        {
            _simDistribution = new PrivateObject(SimDistribution.Instance);
        }

        public void SetTaskArrivalRate(IContinuousDistribution value)
        {
            _simDistribution.SetProperty("TaskArrivalRate", value);
        }

        public void SetWorkerArrivalRate(IContinuousDistribution value)
        {
            _simDistribution.SetProperty("WorkerArrivalRate", value);
        }

        public void SetWorkerLeaveRate(IContinuousDistribution value)
        {
            _simDistribution.SetProperty("WorkerLeaveRate", value);
        }

        public void SetFeedbackDistribution(IContinuousDistribution value)
        {
            _simDistribution.SetProperty("FeedbackDistribution", value);
        }

        public void SetQualityGrade(IContinuousDistribution value)
        {
            _simDistribution.SetProperty("QualityGrade", value);
        }

        public void SetWorkersQualityDistribution(WorkersQualityDistribution value)
        {
            _simDistribution.SetProperty("WorkersQualityDistribution", value);
        }
    }
}