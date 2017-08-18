using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Tasks;

namespace TaskSimulation.Results
{
    public class WorkerExecData
    {
        public double StartAt { get; set; }
        public double EndAt { get; set; }

        private double _busyTime;

        public double BusyTime
        {
            get
            {
                return _busyTime;
            }
            set
            {
                if (value >= 0)
                    _busyTime = value;
                else
                {
                    Log.Err("Value cant be < 0!!");
                }
            }
        }

        public double FreeTime => TotalTime - BusyTime;

        public double TotalTime
        {
            get
            {
                if (EndAt == -1)
                    return SimulateServer.SimulationClock - StartAt;
                return EndAt - StartAt;
            }
        }

        public WorkerExecData()
        {
            StartAt = 0;
            EndAt = -1;
            BusyTime = 0;
        }

        public void UpdateWorkedTime(Task task)
        {
            BusyTime += SimulateServer.SimulationClock - task.StartTime;
        }
    }
}
