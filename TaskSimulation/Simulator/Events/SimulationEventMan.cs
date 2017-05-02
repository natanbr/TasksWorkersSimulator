using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSimulation.Distribution;
using MathNet.Numerics.Distributions;

namespace TaskSimulation.Simulator
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
                AddEvent(new WorkerArrivalEvent(this, 1.1));
        }

        public void AddEvent(AEvent newEvent)
        {
            Log.D($"Adding {newEvent} at time: {newEvent.ArriveTime,-7:##0.###}");
            _events.Enqueue(newEvent);
        }

        public AEvent GetNextEvent()
        {
            if (_events.Count() <= 0) return null;

            var nextEvent =  _events.Dequeue();

            var time = SimulateServer.SimulationClock;

            if (nextEvent is TaskArrivalEvent)
                AddEvent(new TaskArrivalEvent(this, time + DistFactory.TaskArrivalTime.Sample()));

            if (nextEvent is WorkerArrivalEvent)
                AddEvent(new WorkerArrivalEvent(this, time + DistFactory.WorkerArrivalTime.Sample()));


            return nextEvent;
        }
    }
}
