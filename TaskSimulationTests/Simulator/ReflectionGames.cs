using System;
using MathNet.Numerics.Distributions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskSimulation.ChooseAlgorithms;
using TaskSimulation.Distribution;
using TaskSimulation.Simulator.Workers;

namespace TaskSimulationTests.Simulator
{
    [TestClass()]
    public class ReflectionGames
    {
       
        [TestMethod()]
        public void ReflectionContinuousUniformParamsTest()
        {
            var arg1 = new object[] {1, 2};
            var continuousUniform = ReflectIContinuousDistribution.GetDistribution("ContinuousUniform", arg1);
            Assert.IsInstanceOfType(continuousUniform, typeof(ContinuousUniform));
            var sample = continuousUniform.Sample();
            Assert.IsTrue(sample < (int)arg1[1] && sample > (int)arg1[0]);
        }

        [TestMethod()]
        public void ReflectionContinuousUniformExplicitTest()
        {
            var arg1 = 1;
            var arg2 = 2;
            var continuousUniform = ReflectIContinuousDistribution.GetDistribution("ContinuousUniform", arg1, arg2);
            Assert.IsInstanceOfType(continuousUniform, typeof(ContinuousUniform));
            var sample = continuousUniform.Sample();
            Assert.IsTrue(sample < arg2 && sample > arg1);
        }

        [TestMethod()]
        public void ReflectionNormalTest()
        {

            var arg3 = new object[] { 5, 0.01 };
            var normal = ReflectIContinuousDistribution.GetDistribution("Normal", arg3);
            Assert.IsInstanceOfType(normal, typeof(Normal));
            var sample = normal.Sample();
            Assert.IsTrue(4 < sample && sample < 6);
        }

        [TestMethod()]
        public void ReflectionExponentialTest()
        {
            var arg3 = new object[] {5};
            var expo = ReflectIContinuousDistribution.GetDistribution("Exponential", arg3);
            Assert.AreEqual(typeof(Exponential), expo.GetType());
            expo.Sample();
        }

        [TestMethod()]
        public void ReflectionContinuousUniformWithSeedTest()
        {
            var continuousUniformSeed = ReflectIContinuousDistribution.GetDistribution("ContinuousUniform", 1 , 2, new Random());
            Assert.IsInstanceOfType(continuousUniformSeed, typeof(ContinuousUniform));
            var sample = continuousUniformSeed.Sample();
            Assert.IsTrue(1 < sample && sample < 2);
        }

        [TestMethod()]
        public void ReflectionContinuousUniformWithSeedCastObjectTest()
        {
            var continuousUniformSeed = ReflectIContinuousDistribution.GetDistribution("ContinuousUniform", new object[]{1, 2}, new Random());
            Assert.IsInstanceOfType(continuousUniformSeed, typeof(ContinuousUniform));
            var sample = continuousUniformSeed.Sample();
            Assert.IsTrue(1 < sample && sample < 2);
        }

        [TestMethod()]
        public void ReflectionNormalWithSeedCastDecimalTest()
        {
            var normalSeed = ReflectIContinuousDistribution.GetDistribution("Normal", new[] { 5, 0.1 }, new Random());
            Assert.IsInstanceOfType(normalSeed, typeof(Normal));
            var sample = normalSeed.Sample();
            //Assert.IsTrue(2.2 < sample && sample < 1.1);
        }

        [TestMethod()]
        public void ReflectionNormalWithSeedTest()
        {
            var normalSeed = ReflectIContinuousDistribution.GetDistribution("Normal", 5, 0.1, new Random());
            Assert.IsInstanceOfType(normalSeed, typeof(Normal));
            var sample = normalSeed.Sample();
            Assert.IsTrue(4 < sample && sample < 6);
        }
    }
}