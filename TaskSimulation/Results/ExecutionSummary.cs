using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSimulation.Results
{
    public class ExecutionSummary
    {
        public int Seed;
        public int ExecutionTime;

        public decimal TotalWorkersUtilization;
        public decimal TotalSystemUtilization;
        public int FinishedTasksForSingleExecution;
        public int TotalTasksForSingleExecution;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb
                .AppendLine("--------Execution-------")
                .AppendLine($"Workers Utilization:              {TotalWorkersUtilization*100:N2}%")
                .AppendLine($"System Utilization:               {TotalSystemUtilization*100:N2}%")
                .AppendLine($"Finished Tasks:                   {FinishedTasksForSingleExecution}")
                .AppendLine($"Total tasks:                      {TotalTasksForSingleExecution}")
                .AppendLine("");

            return sb.ToString();
        }
    }
}
