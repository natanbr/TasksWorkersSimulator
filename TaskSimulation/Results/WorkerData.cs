using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSimulation.Results
{
    public class WorkerData
    {
        public double AvarageEfficiency { get; set; } = 0;
        public int NumberOfTasksFinished { get; set; } = 0;
        public double TimeAlive { get; set; }
    }
}
