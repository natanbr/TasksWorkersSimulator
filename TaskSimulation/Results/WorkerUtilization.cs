namespace TaskSimulation.Results
{
    public class WorkerUtilization
    {
        public int BusyTime { get; set; }

        public int FreeTime { get; set; }

        public int TotalTime => BusyTime + FreeTime;
    }
}
