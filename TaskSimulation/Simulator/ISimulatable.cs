using TaskSimulation.Simulator.Events;

namespace TaskSimulation.Simulator
{
    public interface ISimulatable
    {
        // void Update<T>(T @event); TODO
        void Update(TaskArrivalEvent @event);
        void Update(TaskFinishedEvent @event);
        void Update(WorkerArrivalEvent @event);
        void Update(WorkerLeaveEvent @event);

    }

}