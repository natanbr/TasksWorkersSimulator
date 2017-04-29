using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSimulation
{
    interface IChooseWorkerAlgo
    {
        List<Worker> ChooseWorkers(List<Worker> activeWorkers, int choose);
    }
}
