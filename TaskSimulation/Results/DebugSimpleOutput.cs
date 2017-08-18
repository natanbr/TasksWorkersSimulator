using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TaskSimulation.Simulator;
using TaskSimulation.Simulator.Events;
using TaskSimulation.Simulator.Tasks;

namespace TaskSimulation.Results
{
    public class DebugSimpleOutput : ISimulatable
    {
        private StringBuilder sb;

        public DebugSimpleOutput()
        {
            sb = new StringBuilder();
        }

        public void Update(TaskArrivalEvent @event)
        {
            var time = @event.ArriveTime;
            var task = @event.Task;
            
            sb.AppendLine($"{time,-4:#0.#}: +T{task.GetTaskId()}");

            var existingWorker = task.GetWorker();
            if (task.GetWorker() != null)
            {
                sb.AppendLine($"{time,-4:#0.#}: T{task.GetTaskId()}->W{existingWorker.GetWorkerID()}");
                sb.Append($"{time,-4:#0.#}: W{existingWorker.GetWorkerID()} [T{existingWorker.GetCurrentTask().GetTaskId()}] ");
                foreach (var t in existingWorker.GetQueue())
                {
                    sb.Append($"T{t.GetTaskId()} ");
                }
                sb.AppendLine();
            }


            task.OnAddedToWorker += w =>
            {
                sb.AppendLine($"{time,-4:#0.#}: T{task.GetTaskId()}->W{w.GetWorkerID()}");
            };

            task.OnTaskAssigned += w =>
            {
                sb.Append($"{time,-4:#0.#}: W{w.GetWorkerID()} [T{w.GetCurrentTask().GetTaskId()}] ");
                foreach (var t in w.GetQueue())
                {
                    sb.Append($"T{t.GetTaskId()} ");
                }
                sb.AppendLine();
            };
        }

        public void Update(TaskFinishedEvent @event)
        {
            var time = @event.ArriveTime;
            var task = @event.Task;
            var worker = @event.Worker;

            sb.AppendLine($"{time,-4:#0.#}: W{worker.GetWorkerID()}FT{task.GetTaskId()}");
        }

        public void Update(WorkerArrivalEvent @event)
        {
            var time = @event.ArriveTime;
            var worker = @event.Worker;

            sb.AppendLine($"{time,-4:#0.#}: +W{worker.GetWorkerID()}");
        }

        public void Update(WorkerLeaveEvent @event)
        {
            var time = @event.ArriveTime;
            var worker = @event.Worker;

            sb.AppendLine($"{time,-4:#0.#}: -W{worker.GetWorkerID()}");
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }
}
