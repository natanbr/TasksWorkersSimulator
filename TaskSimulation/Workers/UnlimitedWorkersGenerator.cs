namespace TaskSimulation
{
    public class UnlimitedWorkersGenerator : IWorkersGenerator
    {
        private long _id = 0;
        public Worker GetNextWorker()
        {
            _id++;
            return new Worker(_id);
        }
    }
}
