
using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MathNet.Numerics.Distributions;
using TaskSimulation;
using TaskSimulation.ChooseAlgorithms;
using TaskSimulation.Distribution;
using TaskSimulation.Results;
using TaskSimulation.Simulator;

namespace GraphicalResults
{
    public partial class Form1 : Form
    {
        private const int _numOfExecutions = 5;
        private ExecutionSummary[] _summaries = new ExecutionSummary[_numOfExecutions];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MainExec();

            chart1.Series.Clear();
            chart1.Titles.Add("Worker utilization");

            var workerUtilization = chart1.Series.Add("Worker utilization");
            chart1.Series[0].BorderWidth = 3;
            chart1.ChartAreas[0].AxisX.Title = "Execution Number #";
            chart1.ChartAreas[0].AxisY.Title = "Value";
            workerUtilization.ChartType = SeriesChartType.Line;

            var i = 0;
            _summaries.ToList().ForEach(elem =>
            {
                // Add point.
                workerUtilization.Points.Add((double) elem.TotalWorkersUtilization);

                i++;
            });
        }

        public void MainExec()
        {

            DistFactory.FeedbackDistribution = new Normal(10, 10);
            DistFactory.QualityGrade = new Normal(10, 10);

            DistFactory.TaskArrivalTime = new Exponential(20);

            DistFactory.GradeSystem = new OriginalGradeCalc();
                        
            for (var i = 0; i < _numOfExecutions; i++)
            {
                _summaries[i] = SingleExecution();
            }

            _summaries.ToList().ForEach(v => Log.I(v.ToString()));
        }

        public static ExecutionSummary SingleExecution()
        {
            var initialNumOfWorkers = 10;
            var maxSimulationTime = 10;

            var simulator = new SimulateServer(maxSimulationTime);

            simulator.Initialize(initialNumOfWorkers);

            simulator.Start();

            var executionStatistics = new ExecutionSummary()
            {
                ExecutionTime = maxSimulationTime,

                //TotalWorkersUtilization = simulator.Utilization.GetTotalTaskUtilization(),
                FinishedTasksForSingleExecution = simulator.Utilization.GetNumberOfFinishedTasks(),
                TotalTasksForSingleExecution = simulator.Utilization.GetNumberOfTotalTasks()
            };

            return executionStatistics;
        }
    }
}
