using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
            _sw.WriteLine(GenerateSummery());
            _sw.WriteLine();
            _sw.WriteLine(GetTaskChanges());
            _sw.WriteLine();
            _sw.WriteLine(GenerateAvarageExecutionTime());
            _sw.WriteLine();
            _sw.WriteLine(GenerateAvarageWorkerTime());
            _sw.WriteLine();
            _sw.WriteLine(GenerateSystem());
            _sw.Close();
        }

        public string GenerateSummery()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Execution");
                //.AppendLine($"Workers Utilization:,              {_uData.GetTotalWorkersUtilization() * 100:N2}%")
                //.AppendLine($"System Utilization: ,            {_uData.GetSystemUtilization() * 100:N2}%")
                sb.AppendLine($"Finished Tasks:,,,,,                   {_uData.TasksWorkStatistics.GetFinishedTasks() }")
                .AppendLine($"Created Tasks:,,,,,                    {_uData.TasksWorkStatistics.GetCreatedTasks()}")
                .AppendLine($"Workers Avarage Pricessing time:,,,,,  {_uData.TasksWorkStatistics.GetLastAvarageProcessingTime()}")
                .AppendLine($"Workers Avarage Waiting Time:,,,,,     {_uData.TasksWorkStatistics.GetLastAvarageWaitingTime()}")
                .AppendLine($"Workers Avarage Execution Time:,,,,,   {_uData.TasksWorkStatistics.GetLastAvarageExecutionTime()}")
                .AppendLine($"Workers Avarage Parsent Of Wait:,,,,,  {_uData.TasksWorkStatistics.GetParsentOfWaitTime()}")
                .AppendLine($"Workers Parsent Of Work Time:,,,,,     {_uData.TasksWorkStatistics.GetParsentOfworkTime() * 100}%")
                .AppendLine($"Workers Avarage Efficiency:,,,,,       {_uData.WorkersStatistics.GetLastAvarageWorkersEfficiency()}")
                .AppendLine($"Total Workers:,,,,,                    {_uData.WorkersStatistics.GetNumberOfTotalWorkers()}")
                .AppendLine($"GetAvarageUtilizationPerWorker:,,,,,   {_uData.WorkerStatisticsGrouping.GetAvarageUtilizationPerWorker()}")
                .AppendLine($"GetAvarageQueuePerWorker:,,,,,         {_uData.WorkerStatisticsGrouping.GetAvarageQueuePerWorker()}")
                .AppendLine($"GetAvarageWaitingTimePerWorker:,,,,,   {_uData.WorkerStatisticsGrouping.GetAvarageWaitingTimePerWorker()}");

            return sb.ToString();
        }

        public string GenerateAvarageExecutionTime()
        {
            var sb = new StringBuilder();
            var executionTime = _uData.TasksWorkStatistics.GetAvarageExecutionTime();
            var processingTime = _uData.TasksWorkStatistics.GetAvarageProcessingTime();
            var waitingTime = _uData.TasksWorkStatistics.GetAvarageWaitingTime();

            sb.AppendLine($"Average Tasks' Execution Time");
            sb.AppendLine($"Time, Average Execution Time, Average Processing Time, Average waiting  Time");
            for (var i = 0; i < executionTime.Count; i++)
            {
                var et = executionTime[i];
                var pt = processingTime[i];
                var wt = waitingTime[i];

                if (et.Item1 == pt.Item1 && et.Item1 == wt.Item1)
                    sb.AppendLine($"{et.Item1},{et.Item2}, {pt.Item2}, {wt.Item2}");
                else
                    sb.AppendLine($"{et.Item1},{et.Item2}");
            }

            return sb.ToString();
        }

        public string GetTaskChanges()
        {
            var sb = new StringBuilder();
            var workersEfficiency = _uData.TasksWorkStatistics.GetTaksChanges();

            sb.AppendLine($"Log of added and arriving (+1) and finished (-1) task events");
            sb.AppendLine($"Time, Tasks In Queue");

            foreach (var ef in workersEfficiency)
            {
                sb.AppendLine($"{ef.Item1},{ef.Item2}");
            }

            return sb.ToString();
        }

        public string GenerateAvarageWorkerTime()
        {
            var sb = new StringBuilder();
            var workersEfficiency = _uData.WorkersStatistics.GetAvarageWorkersEfficiency();

            sb.AppendLine($"Avarage Workers' Efficiency (Sum Avarage workers efficiency/ Num of workers) ");
            sb.AppendLine($"Time, Average Efficiency");

            foreach (var ef in workersEfficiency)
            {
                sb.AppendLine($"{ef.Item1},{ef.Item2}");
            }

            return sb.ToString();
        }

        public string GenerateSystem()
        {
            var sb = new StringBuilder();
            var systemUtilization2 = _uData.SystemUtilizationStatistics2.GetSystemUtilization();
            var avarageSystemUtilization2 = _uData.SystemUtilizationStatistics2.GetAvarageSystemUtilization();

            sb.AppendLine($"System Utilization = (Number of working workers/ Num of workers) ");
            sb.AppendLine($"Avarage System Utilization = Avarage of System Utilization");
            sb.AppendLine($"Time, System Utilization, Avarage");
            sb.AppendLine(ListToCSVFormat(systemUtilization2, avarageSystemUtilization2));
            return sb.ToString();
        }


        private string ListToCSVFormat<TA1, TA2, TB2>(List<Tuple<TA1, TA2>> listA, List<Tuple<TA1, TB2>> listB)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < listA.Count; i++)
            {
                var la = listA[i];
                var lb = listB[i];

                if (!la.Item1.Equals(lb.Item1))
                    throw new Exception("un-synchronized results");
                
                sb.AppendLine($"{la.Item1},{la.Item2},{lb.Item2}");
            }

            return sb.ToString();
        }
    }
}
