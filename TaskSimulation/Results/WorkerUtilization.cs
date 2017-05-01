namespace TaskSimulation.Results
{
    public class WorkerUtilization
    {
        public double BusyTime { get; set; }

        public double FreeTime { get; set; }

        public double TotalTime => BusyTime + FreeTime;
    }
}
