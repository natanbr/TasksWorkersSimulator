using TaskSimulation.Workers;

namespace TaskSimulation.ChooseAlgorithms
{
    public interface IGradeCalcAlgo
    {
        int GetFinalGrade(Grade worker);

        int InitialGrade();
        Grade TaskAdded(Grade grade, int taskInQueue);
        Grade TaskRemoved(Grade grade, int taskInQueue, long responseTime);

        Grade GenerateRandomGrade(Grade worker);
        int GetMaxNumberOfTasks();
    }
}
