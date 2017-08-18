using System;
using System.Collections.Generic;
using System.Linq;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Workers;

namespace TaskSimulation.ChooseAlgorithms
{
    public class ChooseLowestGrade : IChooseWorkerAlgo
    {
        public List<Worker> ChooseWorkers(List<Worker> activeWorkers, int chooseNum)
        {
            var workers = activeWorkers
                .OrderBy(w => w.Grade.TotalGrade)
                .ThenBy(w => w.Grade.NumberOfTasksGrade)
                .Take(chooseNum).ToList();

            return workers;
        }
    }
}
