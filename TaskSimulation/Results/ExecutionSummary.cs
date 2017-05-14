using System.Text;

namespace TaskSimulation.Results
{
    public class ExecutionSummary
    {
        public int Seed;
        public double ExecutionTime;

        public double TotalWorkersUtilization;
        public double TotalSystemUtilization;
        public int FinishedTasksForSingleExecution;
        public int TotalTasksForSingleExecution;
        public double TotalTasksWait;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb
                .AppendLine("--------Execution-------")
                .AppendLine($"Workers Utilization:              {TotalWorkersUtilization*100:N2}%")
                .AppendLine($"System Utilization:               {TotalSystemUtilization*100:N2}%")
                .AppendLine($"Tasks Wait:                       {TotalTasksWait * 100:N2}%")
                .AppendLine($"Finished Tasks:                   {FinishedTasksForSingleExecution}")
                .AppendLine($"Total tasks:                      {TotalTasksForSingleExecution}")
                .AppendLine("");

            return sb.ToString();
        }
    }
}
