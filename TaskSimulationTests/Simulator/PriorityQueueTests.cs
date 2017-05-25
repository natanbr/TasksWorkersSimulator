using System;
using MathNet.Numerics.Distributions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskSimulation;
using TaskSimulation.ChooseAlgorithms;
using TaskSimulation.Distribution;
using TaskSimulation.Results;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Events;

namespace TaskSimulationTests.Simulator
{
    [TestClass()]
    public class PriorityQueueTests
    {
        [TestMethod()]
        public void PriorityQueueTest()
        {
            var pq = new PriorityQueue<AEvent>();

            pq.Enqueue(new WorkerLeaveEvent(null, 10));
            pq.Enqueue(new TaskArrivalEvent(null, 15.2));
            pq.Enqueue(new TaskArrivalEvent(null, 11));
            pq.Enqueue(new WorkerArrivalEvent(null, 2));
            pq.Enqueue(new TaskArrivalEvent(null, 1.1));
            pq.Enqueue(new TaskFinishedEvent(null, null, 1.01));
            pq.Enqueue(new TaskArrivalEvent(null, 1.2));
            pq.Enqueue(new TaskArrivalEvent(null, 102));

            Assert.AreEqual(1.01,pq.Dequeue().ArriveTime);
            Assert.AreEqual(1.1,pq.Dequeue().ArriveTime);
            Assert.AreEqual(1.2,pq.Dequeue().ArriveTime);
            Assert.AreEqual(2,pq.Dequeue().ArriveTime);
            Assert.AreEqual(10,pq.Dequeue().ArriveTime);
            Assert.AreEqual(11, pq.Dequeue().ArriveTime);
            Assert.AreEqual(15.2, pq.Dequeue().ArriveTime);
            Assert.AreEqual(102, pq.Dequeue().ArriveTime);
        }
    }
}