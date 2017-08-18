using System;
using TaskSimulation.Simulator.Workers;

namespace TaskSimulation.Simulator.Tasks
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

        private Worker _addedTo;

        public event Action<Worker> OnTaskAssigned;
        public event Action<Worker> OnAddedToWorker;

        public Task()
        {
            EndTime = NOT_STARTED;
            StartTime = NOT_STARTED;
            _eventCode = TASK_ID++;
        }

        public int GetTaskId()
        {
            return _eventCode;
        }

        public void SetStateAddedTo(Worker worker)
        {
            _addedTo = worker;
            OnAddedToWorker?.Invoke(worker);
        }

        public void SetStateAssignedBy(Worker worker)
        {
            StartTime = SimulateServer.SimulationClock;

            if (StartTime > EndTime && EndTime != NOT_STARTED)
                Log.Err($"StartTime > EndTime {StartTime} {EndTime}");

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

        public Worker GetWorker()
        {
            return _addedTo;
        }
    }
}
