﻿using System;
using System.Collections.Generic;
using System.Linq;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Workers;

namespace TaskSimulation.ChooseAlgorithms
{
    public class ChooseHighestGrade : IChooseWorkerAlgo
    {
        public List<Worker> ChooseWorkers(List<Worker> activeWorkers, int chooseNum)
        {
            var workers = activeWorkers
                .OrderByDescending(w => w.Grade.TotalGrade)
                .ThenByDescending(w => w.Grade.NumberOfTasksGrade)
                .Take(chooseNum).ToList();

            return workers;
        }
    }
}
