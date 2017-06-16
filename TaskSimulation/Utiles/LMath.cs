using System;

namespace TaskSimulation.Utiles
{
    public static class LMath
    {
        public static double AverageIncrementalSize(double oldAverage, double oldSize, double newValue, double newSize)
        {
            if (Math.Abs((oldSize + newSize)) <= 0.00001)
                return oldAverage;

            Log.D("Grade Updated: average_new = (average_old * oldSize + newValue) / total_Work_Time = \n " +
                $"({oldAverage} * {oldSize} + {newValue}) / {oldSize + newSize}" +
                $"={(oldAverage *  oldSize +   newValue) /  (oldSize + newSize)}");

            return ((oldAverage * oldSize) + newValue) / (oldSize + newSize);
        }

        public static double Average(double oldAverage, double oldSize, double newValue, double newSize)
        {
            if (Math.Abs((oldSize + newSize)) <= 0.00001)
                return oldAverage;

            Log.D("Grade Updated: average_new = (average_old * oldSize + newValue) / total_Work_Time = \n " +
                $"({oldAverage} * {oldSize} + {newValue}) / {oldSize + newSize}" +
                $"={(oldAverage * oldSize + newValue) / (newSize)}");

            return ((oldAverage * oldSize) + newValue) / (newSize);
        }
    }
}
