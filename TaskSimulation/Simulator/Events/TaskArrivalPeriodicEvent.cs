using MathNet.Numerics.Distributions;
using TaskSimulation.Distribution;

namespace TaskSimulation.Simulator.Events
{
    public class TaskArrivalPeriodicEvent : TaskArrivalEvent, IPeriodicEvent
    {
        public TaskArrivalPeriodicEvent(SimulationEventMan eventMan, double arriveAt) : base(eventMan, arriveAt) { }

        public IContinuousDistribution GetRate()
        {
            return SimDistribution.I.TaskArrivalRate;
        }
    }
}