namespace TaskSimulation.Simulator.Workers
{
    public interface IWorkersGenerator
    {
        Worker GetNextWorker();
    }
}