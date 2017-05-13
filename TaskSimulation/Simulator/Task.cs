using System;
using TaskSimulation.Distribution;
using TaskSimulation.Results;
using TaskSimulation.Simulator.Events;

namespace TaskSimulation.Simulator
{
    public class Task
    {
        #region Consts

        public static int TASK_ID = 0;
        public const int NOT_STARTED = -1;

        #endregion
        
        private readonly int _eventCode;

        public double CreatedTime { get; set; }
        public double StartTime { get; private set; }
        public double EndTime { get; private set; }

        public event Action<Worker> OnTaskAssigned;

        public Task()
        {
            EndTime = NOT_STARTED;
            StartTime = NOT_STARTED;
            _eventCode = TASK_ID++;
        }

        public void AssignedBy(Worker worker)
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
