using System;
using TaskSimulation.Distribution;
using TaskSimulation.Results;

namespace TaskSimulation.Simulator
{
    public class Task : ISimulatable, IStatisticCollectorVisitor
    {
        public static int TASK_ID = 0;
        public const int NOT_STARTED = -1;
        //public int Progress { get; private set; }

        private int _eventCode;

        public long CreatedTime { get; private set; }
        public long StartTime { get; private set; }

        public long EndTime { get; private set; }

        //public List<Worker> Assigned { get; private set; }

        public event Action<Task> OnTaskComplite;
        public event Action<Task> OnTaskAssigned;

        public Task()
        {
           // Assigned = new List<Worker>();

            EndTime = NOT_STARTED;
            StartTime = NOT_STARTED;
            CreatedTime = SimulateServer.SimulationClock;
            _eventCode = TASK_ID++;
        }

        public void Assign(Worker worker)
        {
            //Assigned.Add(worker);

            StartTime = SimulateServer.SimulationClock;

            //PrintAssignment();

            OnTaskAssigned?.Invoke(this);
        }

        /*private void PrintAssignment()
        {
            Console.Write($"{this} Assigned to");

            foreach (var worker in Assigned)
                Console.Write($"{worker} ");
        }*/

        public override string ToString()
        {
            return $"Task: {_eventCode}" ;
        }

        public void Update()
        {
            if (StartTime == SimulateServer.SimulationClock)
                return; // The task has been just created, no need to update

            var isComplite = DistFactory.TaskCompliteRate.Test();

            if (isComplite)
            {
                // Log.D($"<T< {this} finished");

                EndTime = SimulateServer.SimulationClock;

                OnTaskComplite?.Invoke(this);
            }

        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
