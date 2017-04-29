using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSimulation
{
    public class UnlimitedWorkersGenerator : IWorkersGenerator
    {
        public Worker GetNextWorker()
        {
            return new Worker();
        }
    }
}
