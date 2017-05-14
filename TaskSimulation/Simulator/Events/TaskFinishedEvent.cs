using MathNet.Numerics.Distributions;
using TaskSimulation.Distribution;
using TaskSimulation.Simulator.Tasks;
using TaskSimulation.Simulator.Workers;

namespace TaskSimulation.Simulator.Events
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


        public override string ToString()
        {
            if (Worker.IsOnline())
                return $"Finish Event: for {Worker} on {Task}";

            return $"Removing {Task} (Worker {Worker} left)";
        }
    }
}