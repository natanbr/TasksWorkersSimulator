﻿using System;

namespace TaskSimulation.Simulator
{
    public class WorkerLeaveEvent : AEvent
    {
        public Worker Worker { get; set; }

        public WorkerLeaveEvent(Worker worker, double arriveAt) : base(arriveAt)
        {
            Worker = worker;
        }

        public override void Accept(ISimulatable visitor)
        {
            visitor.Update(this);
        }

       /* public override string ToString()
        {
            return $"{this.GetType().Name.SpaceCapitals()}";
        }*/
    }
}