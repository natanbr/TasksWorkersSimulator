using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Workers;

namespace TaskSimulation.ChooseAlgorithms
{
    public interface IGradeCalcAlgo
    {
        double GetFinalGrade(Grade worker);

        int InitialGrade();

        Grade UpdateOnTaskAdd(Grade grade);
        Grade UpdateOnTaskRemoved(Grade grade, double responseTime);

        Grade GenerateRandomGrade(Worker worker);
        int GetMaxNumberOfTasks();
    }
}
