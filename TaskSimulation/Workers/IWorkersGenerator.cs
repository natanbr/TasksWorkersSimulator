using System.Collections.Generic;

namespace TaskSimulation
{
    public interface IWorkersGenerator
    {
        Worker GetNextWorker();
    }
}