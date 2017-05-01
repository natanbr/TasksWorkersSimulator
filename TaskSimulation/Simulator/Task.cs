using System;
using TaskSimulation.Distribution;
using TaskSimulation.Results;

namespace TaskSimulation.Simulator
{
    public class Task
    {
        public static int TASK_ID = 0;
        public const int NOT_STARTED = -1;
        //public int Progress { get; private set; }

        private int _eventCode;

        public double CreatedTime { get; private set; }
        public double StartTime { get; private set; }

        public double EndTime { get; private set; }

        public event Action<Worker> OnTaskAssigned;

        public Task(TaskArrivalEvent @event)
        {
            EndTime = NOT_STARTED;
            StartTime = NOT_STARTED;
            CreatedTime = SimulateServer.SimulationClock;
            _eventCode = TASK_ID++;
        }

        public void Assign(Worker worker)
        {
            StartTime = SimulateServer.SimulationClock;

            OnTaskAssigned?.Invoke(worker);
        }

        public void Finished()
        {
            EndTime = SimulateServer.SimulationClock;
        }


        public override string ToString()
        {
            return $"Task: {_eventCode}" ;
        }
    }
}
