namespace TaskSimulation.Results
{
    public interface IStatisticCollectorVisitor
    {
        void Accept(ISimulatable visitor);

    }
}