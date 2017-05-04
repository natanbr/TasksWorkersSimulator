using TaskSimulation.Workers;

namespace TaskSimulation.ChooseAlgorithms
{
    public interface IGradeCalcAlgo
    {
        double GetFinalGrade(Grade worker);

        int InitialGrade();

        Grade UpdateOnTaskAdd(Grade grade);
        Grade UpdateOnTaskRemoved(Grade grade, double responseTime);

        Grade GenerateRandomGrade(Grade worker);
        int GetMaxNumberOfTasks();
    }
}
