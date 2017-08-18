using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskSimulation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSimulation.ChooseAlgorithms;
using TaskSimulation.Distribution;
using TaskSimulation.Simulator;
using TaskSimulationTests.Simulator;

namespace TaskSimulation.Results.Tests
{
    [TestClass()]
    public class WorkersStatisticsTests
    {
        private SimulateServer _simulator;

        [TestMethod()]
        public void AvarageWorkersEfficiencyTest()
        {

            ResultsHelper.GenerateShema();

            SimDistribution.I.GradeSystem = new OriginalGradeCalc();
            SimDistribution.I.GradeSystemChooseMethod = SimDistribution.I.GradeSystem.ChooseMethod();

            _simulator = ResultsHelper.SingleExecution();

            var awf = _simulator.GetResults().WorkersStatistics.GetAvarageWorkersEfficiency();

            var avrg = (double)(0 + 1) / 2;
            Assert.AreEqual(avrg, awf[1].Item2);
            avrg = ((avrg * 2) + (3.0 / 4)) / 2;
            Assert.AreEqual(avrg, Math.Round(awf[2].Item2, 3));
            Assert.AreEqual(avrg, Math.Round(awf[3].Item2, 3));
            Assert.AreEqual(0.929, Math.Round(awf[4].Item2, 3));


        }

    }
}