using TaskSimulation.Distribution;
using TaskSimulation.Simulator.Tasks;
using TaskSimulation.Simulator.Workers;

namespace TaskSimulation.ChooseAlgorithms
{
    public class QueueLengthGradeCalc : IGradeCalcAlgo
    {
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

            return grade;
        }

        public Grade UpdateOnTaskRemoved(Worker worker, Task task) //WORKER TASK
        {
            var grade = worker.Grade;

            var currentTime = Simulator.SimulateServer.SimulationClock;

            grade.NumberOfTasksGrade--; 

            var sumQuereLength = (currentTime - grade.Meta.LastModifiedAt) * grade.NumberOfTasksGrade;

            Log.D($"Grade Updated: sumQuereLength = (currentTime - lastUpdateTime) * QueueLength = " +
                $"(${currentTime} - ${grade.Meta.LastModifiedAt}) * ${grade.NumberOfTasksGrade} = ${sumQuereLength}");

            grade.ResponseGrade += sumQuereLength; // TODO normal

            grade.TotalGrade = grade.ResponseGrade; // TODO add FeedbackGrade, QualityGrade

            grade.Meta.LastModifiedAt = Simulator.SimulateServer.SimulationClock;

            return grade;
        }
    }
}
