using MathNet.Numerics.Distributions;
using TaskSimulation.Distribution;

namespace TaskSimulation.Simulator.Events
{
    public class WorkerArrivalPeriodicEvent : WorkerArrivalEvent, IPeriodicEvent
    {
        public WorkerArrivalPeriodicEvent(SimulationEventMan eventMan, double arriveAt) : base(eventMan, arriveAt)
        {
        }

        public IContinuousDistribution GetRate()
        {
            return SimDistribution.I.WorkerArrivalRate;
        }
    }
}