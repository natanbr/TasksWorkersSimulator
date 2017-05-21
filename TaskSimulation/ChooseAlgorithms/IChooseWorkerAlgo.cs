using System.Collections.Generic;
using TaskSimulation.Simulator.Workers;

namespace TaskSimulation.ChooseAlgorithms
{
    interface IChooseWorkerAlgo
    {
        List<Worker> ChooseWorkers(List<Worker> activeWorkers, int choose);
    }
}
