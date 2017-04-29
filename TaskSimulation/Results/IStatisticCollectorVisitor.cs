namespace TaskSimulation.Results
{
    public interface IStatisticCollectorVisitor
    {
        void Accept(IVisitor visitor);

    }
}