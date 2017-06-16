using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSimulation.Results
{
    public class ResultsFile
    {
        private StreamWriter _sw;
        private Utilization _uData;
        public ResultsFile(string file, Utilization utilization)
        {
            _sw = new StreamWriter(file);
            _uData = utilization;
        }

        public void GenerateCsvFile()
        {
            var summery = WriteSummery();
            _sw.WriteLine(summery);
            _sw.Close();
        }

        public string WriteSummery()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Execution")
                .AppendLine($"Workers Utilization:,              {_uData.GetTotalWorkersUtilization() * 100:N2}%")
                //.AppendLine($"System Utilization: ,              {_uData.GetSystemUtilization() * 100:N2}%")
                .AppendLine($"Tasks Wait:,                       {_uData.TasksWorkStatistics.GetFinishedTasks() * 100:N2}%")
                .AppendLine($"Finished Tasks:,                   {_uData.TasksWorkStatistics.GetCreatedTasks()}")
                .AppendLine($"Total tasks:,                      {_uData.TasksWorkStatistics.TaskWereInWaitList()}")
                .AppendLine($"Workers Avarage Pricessing time:,  {_uData.TasksWorkStatistics.GetAvarageProcessingTime()}")
                .AppendLine($"Workers Avarage Waiting Time:,     {_uData.TasksWorkStatistics.GetAvarageWaitingTime()}")
                .AppendLine($"Workers Avarage Execution Time:,   {_uData.TasksWorkStatistics.GetAvarageExecutionTime()}")
                .AppendLine($"Workers Avarage Parsent Of Wait:,  {_uData.TasksWorkStatistics.GetParsentOfWaitTime()}")
                .AppendLine($"Workers Parsent Of Work Time:,     {_uData.TasksWorkStatistics.GetParsentOfworkTime()}")
                .AppendLine($"Workers Avarage Efficiency:,       {_uData.WorkersStatistics.GetAvarageWorkersEfficiency()}")
                .AppendLine($"Total Workers:,                    {_uData.WorkersStatistics.GetNumberOfTotalWorkers()}");

            return sb.ToString();
        }


    }
}
