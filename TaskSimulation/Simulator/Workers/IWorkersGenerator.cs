namespace TaskSimulation.Simulator.Workers
{
    public interface IWorkersGenerator
    {
        Worker GetNextWorker();

        //void AddWorkers(int numberOfWorkers);
    }
}