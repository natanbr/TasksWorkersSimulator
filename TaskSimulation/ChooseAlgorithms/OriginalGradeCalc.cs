using System;
using TaskSimulation.Distribution;
using TaskSimulation.Workers;

namespace TaskSimulation.ChooseAlgorithms
{
    // TODO convert to factory of factories (each worker will have instance of this class, now all workers use the same instance)
    public class OriginalGradeCalc : IGradeCalcAlgo
    {
        double _runningAvrg = 0.5;
        private const int MAX_NUMBER_OF_TASKS = 10;
        private const int RESPONCE_TIME= 11;
        private const int INITIAL_GRADE = 5;
       

        public double GetFinalGrade(Grade grade)
        {
            var sum = grade.FeedbackGrade + grade.QualityGrade + grade.ResponseGrade + grade.NumberOfTasksGrade;
            var totalGrade = sum / Grade.NUMBER_OF_VARS;

            return totalGrade;
        }

        public Grade UpdateOnTaskAdd(Grade grade)
        {
            grade.NumberOfTasksGrade--;
            grade.TotalGrade = GetFinalGrade(grade);

            return grade;
        }

        public Grade UpdateOnTaskRemoved(Grade grade, double responseTime)
        {
            grade.NumberOfTasksGrade++;
            grade.ResponseGrade = (int)((_runningAvrg) * (RESPONCE_TIME - responseTime) + (1 - _runningAvrg) * grade.ResponseGrade);
            grade.TotalGrade = GetFinalGrade(grade);

            Log.D($"Grade Update: TaskRemoved: (N:{grade.NumberOfTasksGrade}, R:[t={responseTime}]->{grade.ResponseGrade})");

            return grade;
        }

        public Grade GenerateRandomGrade(Grade grade)
        {
            var prevFeedbackGrade = grade.FeedbackGrade;
            var prevQualityGrade = grade.QualityGrade;
            //var prevResponseGrade = grade.ResponseGrade;
            var prevTotal = grade.ResponseGrade;

            var newFeedbackGrade = DistFactory.FeedbackDistribution.Sample();
            var newQualityGrade =  DistFactory.QualityGrade.Sample();
            //var newResponseGrade = DistFactory.ResponseGrade.Sample();

            grade.FeedbackGrade = (int)((_runningAvrg) * newFeedbackGrade + (1 - _runningAvrg) * prevFeedbackGrade);
            grade.QualityGrade =  (int)((_runningAvrg) * newQualityGrade +  (1 - _runningAvrg) * prevQualityGrade);
            //grade.ResponseGrade = (int)((_runningAvrg) * newResponseGrade + (1 - _runningAvrg) * prevResponseGrade);
            var total = GetFinalGrade(grade);

            Log.D($"Grade Updated (R, F, Q, N, G)" +
                              $" Prev (x,{prevFeedbackGrade},{prevQualityGrade},x,{prevTotal}) " +
                              $"Rand (x,{(int)newFeedbackGrade},{(int)newQualityGrade},x ,x) " +
                              $"final({grade.ResponseGrade},{grade.FeedbackGrade},{grade.QualityGrade},{grade.NumberOfTasksGrade},{total})");
            grade.TotalGrade = total;

            return grade;
        }

        public int GetMaxNumberOfTasks()
        {
            return MAX_NUMBER_OF_TASKS;
        }

        public int InitialGrade()
        {
            return INITIAL_GRADE;
        }

    }
}
