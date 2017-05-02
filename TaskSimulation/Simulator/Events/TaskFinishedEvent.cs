using System;

namespace TaskSimulation.Simulator
{
    public class TaskFinishedEvent : AEvent
    {
        public Task Task { get; set; }
        public Worker Worker { get; set; }


        public TaskFinishedEvent(Task task, Worker worker, double arriveAt) : base(arriveAt)
        {
            Task = task;
            Worker = worker;
        }

        public override void Accept(ISimulatable visitor)
        {
            visitor.Update(this);
        }

        /*public override string ToString()
        {
            return $"{this.GetType().Name.SpaceCapitals()}";
        }*/
    }
}