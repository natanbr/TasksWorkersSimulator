using TaskSimulation.Simulator;

namespace TaskSimulation.Results
{
    public class WorkerStatistics
    {
        public double StartAt { get; set; }
        public double EndAt { get; set; }

        public double BusyTime { get; set; }

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
