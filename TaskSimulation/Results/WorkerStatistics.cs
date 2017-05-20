using TaskSimulation.Simulator;

namespace TaskSimulation.Results
{
    public class WorkerStatistics
    {
        public double StartAt { get; set; }
        public double EndAt { get; set; }

        private double _busyTime { get; set; }

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

        public WorkerStatistics()
        {
            StartAt = 0;
            EndAt = -1;
            BusyTime = 0;
        }
    }
}
