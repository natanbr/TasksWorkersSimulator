using TaskSimulation.Distribution;
using TaskSimulation.Simulator.Tasks;
using TaskSimulation.Simulator.Workers;
using TaskSimulation.Utiles;

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
            grade.NumberOfTasksGrade++; // TODO add to metadata as NumberOfTasks

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

        public IChooseWorkerAlgo ChooseMethod()
        {
            return new ChooseLowestGrade();
        }

        /// <summary>
        /// Queue grade re-calculations
        /// QueueTime = (Delta time * Queue length) 
        /// QueueAvarage = TODO Need to normalize the QueueTime
        /// </summary>
        /// <param name="grade"></param>
        private void UpdateQueueGrade(ref Grade grade)
        {
            // TODO ask, is this Queue length or Total execution time
            var currentTime = Simulator.SimulateServer.SimulationClock;

            var workingTime = grade.Meta.WorkingTime;

            var newDeltaTime = currentTime - grade.Meta.LastModifiedAt;

            if (newDeltaTime <= 0)
                return;

            grade.Meta.WorkingTime += newDeltaTime;

            var currentQeueuValue = (grade.NumberOfTasksGrade - TASKS_IN_PROSS);

            // Avarage
            grade.ResponseGrade = LMath.Average(grade.ResponseGrade, workingTime, currentQeueuValue, newDeltaTime);

            grade.TotalGrade = grade.ResponseGrade; // TODO add FeedbackGrade

            grade.Meta.LastModifiedAt = Simulator.SimulateServer.SimulationClock;
        }
    }
}
