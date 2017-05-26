using MathNet.Numerics.Distributions;

namespace TaskSimulation.Simulator.Events
{
    public interface IPeriodicEvent
    {
        IContinuousDistribution GetRate();
    }
}