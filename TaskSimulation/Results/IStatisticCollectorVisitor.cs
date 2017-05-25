using TaskSimulation.Simulator;

namespace TaskSimulation.Results
{
    public interface IStatisticCollectorVisitor
    {
        void Accept(ISimulatable visitor);

    }
}