namespace TaskSimulation.Workers
{
    public class Grade
    {
        public const int NUMBER_OF_VARS = 4;

        public int TotalGrade;

        public int ResponseGrade;
        public int FeedbackGrade;
        public int QualityGrade;
        public int NumberOfTasksGrade;

        public override string ToString()
        {
            return $"Grade: {TotalGrade} (R:{ResponseGrade},F:{FeedbackGrade},Q:{QualityGrade},N:{NumberOfTasksGrade})";
        }
    }
}
