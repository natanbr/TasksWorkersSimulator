using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskSimulation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSimulation.Results.Tests
{
    [TestClass()]
    public class SystemUtilizationStatistics2Tests
    {
        [TestMethod()]
        public void GetAvarageSystemUtilizationTest1()
        {
            var s = new SystemUtilizationStatistics2();
            s.Data.AddWorker(0);
            s.Data.AddWorker(0);
            s.Data.AddWorkingWorker(1);
            s.Data.AddWorkingWorker(2);
            s.Data.CalculateUtilization(3);

            var sau = s.GetAvarageSystemUtilization();

            Assert.AreEqual(0, sau[0].Item2);
            Assert.AreEqual((double) 0/2, sau[1].Item2);
            Assert.AreEqual((double) (0 + 0.5*1)/(2 + 2), sau[2].Item2);
            Assert.AreEqual((double) (0 + 0.5*1 + 1*2)/(2 + 2 + 2), sau[3].Item2);
        }

        [TestMethod()]
        public void GetSystemUtilizationTest1()
        {
            var s = new SystemUtilizationStatistics2();
            s.Data.AddWorker(0);
            s.Data.AddWorker(0);
            s.Data.AddWorkingWorker(1);
            s.Data.AddWorkingWorker(2);
            s.Data.CalculateUtilization(3);

            var su = s.GetSystemUtilization();
            
            Assert.AreEqual(0, su[0].Item2);
            Assert.AreEqual((double)0.5, su[1].Item2);
            Assert.AreEqual((double)1 , su[2].Item2);
            Assert.AreEqual((double)1, su[3].Item2);
        }
    }
}