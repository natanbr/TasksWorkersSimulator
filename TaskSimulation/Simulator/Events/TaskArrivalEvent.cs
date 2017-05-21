using TaskSimulation.Simulator.Tasks;

namespace TaskSimulation.Simulator.Events
{
    public class TaskArrivalEvent : AEvent
    {
        public Task Task { get; set; }
        public SimulationEventMan EventMan { get; }

        public TaskArrivalEvent(SimulationEventMan eventMan, double arriveAt) : base(arriveAt)
        {
            EventMan = eventMan;
            Task = new Task();
        }

        public override void Accept(ISimulatable visitor)
        {
            visitor.Update(this);
        }

        public override string ToString()
        {
            return base.ToString() + $" {Task}";
        }
    }
}