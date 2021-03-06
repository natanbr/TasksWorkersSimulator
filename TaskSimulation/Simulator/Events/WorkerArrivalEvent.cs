﻿using TaskSimulation.Simulator.Workers;

namespace TaskSimulation.Simulator.Events
{
    public class WorkerArrivalEvent : AEvent
    {
        public Worker Worker { get; set; }
        public SimulationEventMan EventMan { get; }

        public WorkerArrivalEvent(SimulationEventMan eventMan, double arriveAt) : base(arriveAt)
        {
            EventMan = eventMan;
        }

        public override void Accept(ISimulatable visitor)
        {
            visitor.Update(this);
        }
    }
}