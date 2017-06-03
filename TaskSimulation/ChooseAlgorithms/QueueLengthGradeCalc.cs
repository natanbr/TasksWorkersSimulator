using TaskSimulation.Distribution;
using TaskSimulation.Simulator.Tasks;
using TaskSimulation.Simulator.Workers;

namespace TaskSimulation.ChooseAlgorithms
{
    public class QueueLengthGradeCalc : IGradeCalcAlgo
    {
        const int TASKS_IN_PROSS = 1;

        public Grade InitialGrade()
        {
            var grade = new Grade()
            {
                FeedbackGrade = 0,
                QualityGrade = 0,
                ResponseGrade = 0,
                NumberOfTasksGrade = 0,
            };

            return grade;
        }

        public Grade UpdateOnTaskAdd(Grade grade)
        {
            grade.NumberOfTasksGrade++;

            UpdateQueueGrade(ref grade);

            return grade;
        }

        public Grade UpdateOnTaskRemoved(Worker worker, Task task) //WORKER TASK
        {
            var grade = worker.Grade;

            UpdateQueueGrade(ref grade);

            grade.NumberOfTasksGrade--; 

            return grade;
        }

        /// <summary>
        /// Queue grade re-calculations
        /// QueueTime = (Delta time * Queue length) 
        /// QueueAvarage = TODO Need to normalize the QueueTime
        /// </summary>
        /// <param name="grade"></param>
        private void UpdateQueueGrade(ref Grade grade)
        {
            var currentTime = Simulator.SimulateServer.SimulationClock;

            var sumQueueLength = (currentTime - grade.Meta.LastModifiedAt) * (grade.NumberOfTasksGrade - TASKS_IN_PROSS);

            Log.D("Grade Updated: sumQuereLength = (currentTime - lastUpdateTime) * QueueLength = " +
                $"({currentTime} - {grade.Meta.LastModifiedAt}) * {grade.NumberOfTasksGrade - TASKS_IN_PROSS} = {sumQueueLength}");

            grade.ResponseGrade += sumQueueLength; // TODO normalize

            grade.TotalGrade = grade.ResponseGrade; // TODO add FeedbackGrade, QualityGrade

            grade.Meta.LastModifiedAt = Simulator.SimulateServer.SimulationClock;
        }
    }
}
