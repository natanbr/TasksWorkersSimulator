namespace TaskSimulation.Simulator.Workers
{
    public class Grade
    {
        public const int NUMBER_OF_VARS = 4;

        public double TotalGrade;

        public double ResponseGrade;
        public double FeedbackGrade;
        public double QualityGrade;
        public int NumberOfTasksGrade { get; set; }

        public MetaData Meta = new MetaData();

        public override string ToString()
        {
            return $"Grade: {TotalGrade,-4:0.##} (R:{ResponseGrade,-4:0.##},F:{FeedbackGrade,-4:0.##},Q:{QualityGrade,-4:0.##},N:{NumberOfTasksGrade})";
        }

        public class MetaData
        {
            public double LastModifiedAt;
            public double WorkingTime;
        }
    }
}
