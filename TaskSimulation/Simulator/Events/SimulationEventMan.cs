using TaskSimulation.Distribution;

namespace TaskSimulation.Simulator.Events
{
    public class SimulationEventMan
    {
        private PriorityQueue<AEvent> _events;

        public bool IsEmpty()
        {
            return (_events == null || _events.Count() <= 0);
        }

        public SimulationEventMan()
        {
            _events = new PriorityQueue<AEvent>();
        }

        public void InitializeEvents(int tasks, int workers)
        {
            for(var t = 0; t < tasks; t++)
                AddEvent(new TaskArrivalEvent(this, 0));

            for (var w = 0; w < workers; w++)
                AddEvent(new WorkerArrivalEvent(this, 0));
        }

        public void AddEvent(AEvent newEvent)
        {
            var msg = $"Adding {newEvent} at time: {newEvent.ArriveTime,-7:##0.###}";
            Log.D(msg);
            _events.Enqueue(newEvent);
        }

        public AEvent GetNextEvent()
        {
            if (_events.Count() <= 0) return null;

            var deqEvent =  _events.Dequeue();

            if (deqEvent is TaskArrivalEvent)
                AddEvent(new TaskArrivalEvent(this, deqEvent.ArriveTime + SimDistribution.I.TaskArrivalTime.Sample()));

            if (deqEvent is WorkerArrivalEvent)
                AddEvent(new WorkerArrivalEvent(this, deqEvent.ArriveTime + SimDistribution.I.WorkerArrivalTime.Sample()));


            return deqEvent;
        }
    }
}
