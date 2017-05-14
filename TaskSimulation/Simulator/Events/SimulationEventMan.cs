using System;
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

        public void InitializeGenesisEvents(int tasks = 1, int workers = 1)
        {
            // Add the base periodic events
            AddEvent(new TaskArrivalPeriodicEvent(this, 0));
            AddEvent(new WorkerArrivalPeriodicEvent(this, 0));

            // Add the extra workers and tasks as events
            for (var t = 0; t < tasks - 1; t++)
                AddEvent(new TaskArrivalEvent(this, 0));

            for (var w = 0; w < workers - 1; w++)
                AddEvent(new WorkerArrivalEvent(this, 0));
        }

        public void AddEvent(AEvent newEvent)
        {
            var msg = $"Adding {newEvent} at time: {newEvent.ArriveTime,-7:##0.###}";

            if (newEvent.ArriveTime >= double.MaxValue)
                Log.Err("!!! time up !!");

            Log.D(msg);

            _events.Enqueue(newEvent);

            var cons = _events.IsConsistent();
            if (!cons)
                Log.Err("not consist");
        }

        public AEvent GetNextEvent()
        {
            if (_events.Count() <= 0) return null;

            var deqEvent = _events.Dequeue();

            CreateNextPeriodicEvent(deqEvent);

            return deqEvent;
        }

        private void CreateNextPeriodicEvent(AEvent deqEvent)
        {
            if (!(deqEvent is IPeriodicEvent))
                return;

            var eventType = deqEvent.GetType();
            var nextEventTime = deqEvent.ArriveTime + ((IPeriodicEvent) deqEvent).GetRate().Sample();
            var newEvent = Activator.CreateInstance(eventType, this, nextEventTime);
            AddEvent(newEvent as AEvent);
        }
    }
}
