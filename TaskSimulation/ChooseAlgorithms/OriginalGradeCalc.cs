using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Tasks;
using TaskSimulation.Simulator.Workers;

namespace TaskSimulation.ChooseAlgorithms
{
    // TODO convert to factory of factories (each worker will have instance of this class, now all workers use the same instance)
    public class OriginalGradeCalc : IGradeCalcAlgo
    {
        double _runningAvrg = 0.5;
        private const int MAX_NUMBER_OF_TASKS = 10;
        private const int INITIAL_GRADE = 5;

        public Grade InitialGrade()
        {
            var grade = new Grade()
            {
                FeedbackGrade = INITIAL_GRADE,
                QualityGrade = INITIAL_GRADE,
                ResponseGrade = INITIAL_GRADE,
                NumberOfTasksGrade = MAX_NUMBER_OF_TASKS,
            };

            grade.TotalGrade = CalculateFinalGrade(grade);

            return grade;
        }

        public Grade UpdateOnTaskAdd(Grade grade)
        {
            grade.NumberOfTasksGrade--;
            grade.TotalGrade = CalculateFinalGrade(grade);

            return grade;
        }

        //public Grade UpdateOnTaskRemoved(Grade grade, double responseTime)
        public Grade UpdateOnTaskRemoved(Worker worker, Task task)
        {
            var grade = worker.Grade;

            grade.NumberOfTasksGrade++;

            grade = CalculateNewGrade(worker, task);

            return grade;
        }

        private Grade CalculateNewGrade(Worker worker, Task task)
        {
            var grade = worker.Grade;
            var prevFeedbackGrade = grade.FeedbackGrade;
            var prevQualityGrade = grade.QualityGrade;
            var prevResponseGrade = grade.ResponseGrade;
            var prevTotal = grade.TotalGrade;
            var timeNow = SimulateServer.SimulationClock;

            var newFeedbackGrade = worker.Distribution.Feedback.Sample();
            var newQualityGrade = worker.Distribution.JobQuality.Sample();
            var responseTime = timeNow - task.StartTime;

            grade.FeedbackGrade = (int)((_runningAvrg) * newFeedbackGrade + (1 - _runningAvrg) * prevFeedbackGrade);
            grade.QualityGrade =  (int)((_runningAvrg) * newQualityGrade +  (1 - _runningAvrg) * prevQualityGrade);
            grade.ResponseGrade = (int)((_runningAvrg) * (responseTime) + (1 - _runningAvrg) * grade.ResponseGrade);
            
            grade.TotalGrade = CalculateFinalGrade(grade);

            Log.D($"Response Calculations: R:[t={responseTime}]->{grade.ResponseGrade})");

            Log.D($"Grade Updated (R, F, Q, N, G)" +
                              $" Prev ({prevResponseGrade},{prevFeedbackGrade},{prevQualityGrade},x,{prevTotal})" +
                              $" New  ({grade.ResponseGrade},{(int)newFeedbackGrade},{(int)newQualityGrade},x ,x)" +
                              $" Final({grade.ResponseGrade},{grade.FeedbackGrade},{grade.QualityGrade},{grade.NumberOfTasksGrade},{grade.TotalGrade})");

            return grade;
        }

        private double CalculateFinalGrade(Grade grade)
        {
            var sum = grade.FeedbackGrade + grade.QualityGrade + grade.ResponseGrade + grade.NumberOfTasksGrade;
            var totalGrade = sum / Grade.NUMBER_OF_VARS;

            return totalGrade;
        }

        public int GetMaxNumberOfTasks()
        {
            return MAX_NUMBER_OF_TASKS;
        }  
    }
}
