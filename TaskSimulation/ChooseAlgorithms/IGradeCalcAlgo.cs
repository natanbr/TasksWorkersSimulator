using TaskSimulation.Workers;

namespace TaskSimulation.ChooseAlgorithms
{
    public interface IGradeCalcAlgo
    {
        double GetFinalGrade(Grade worker);

        int InitialGrade();

        Grade TaskAdded(Grade grade);
        Grade TaskRemoved(Grade grade, double responseTime);

        Grade GenerateRandomGrade(Grade worker);
        int GetMaxNumberOfTasks();
    }
}
