using System;

namespace TaskSimulation.Simulator
{
    public abstract class AEvent : IComparable<AEvent>
    {
        public double ArriveTime { get; }
       
        protected AEvent(double arriveAt)
        {
            ArriveTime = arriveAt;
        }

        //public abstract void Update();

        public int CompareTo(AEvent other)
        {
            return ArriveTime.CompareTo(other.ArriveTime);
        }

        public abstract void Accept(ISimulatable visitor);
    }
}