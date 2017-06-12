using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskSimulation.Simulator.Workers;
using TaskSimulation.Utiles;

namespace TaskSimulationTests.Simulator
{
    [TestClass()]
    public class SaveLoadTests
    {
        string path = @"c:\Users\nbraslav\Desktop\v1.xml";

        [TestMethod()]
        public void SaveTest()
        {
            var shema = InputXmlShema.Default;

            AddComments();

            XmlUtils.Save(path, shema);

            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod()]
        public void SaveEmptyFileTest()
        {
            XmlUtils.Save(path, InputXmlShema.Default);
            var xml = XmlUtils.Load<InputXmlShema>(path);
            Assert.IsTrue(xml?.V != null);
        }

        [TestMethod()]
        public void SaveV1()
        {
            var shema = InputXmlShema.Default;
            var exec1 = shema.Executions[0];
            exec1.InitialNumOfWorkers = 100;
            exec1.MaxSimulationTime = 24*60*60;
            exec1.TaskArrivalRate.Type = "Exponential";
            exec1.TaskArrivalRate.Params = new double[] {0.43};
            exec1.WorkerArrivalRate.Type = "ContinuousUniform";
            exec1.WorkerArrivalRate.Params = new double[] { 90000, 90000 };
            exec1.WorkerLeaveRate.Type = "ContinuousUniform";
            exec1.WorkerLeaveRate.Params = new double[] { 90000, 90000 };
            var wQuality = exec1.WorkersQualityDistribution;
            wQuality.FeedbackMean.Type = "ContinuousUniform";
            wQuality.FeedbackMean.Params = new double[] { 7, 7 };
            wQuality.FeedbackStd.Type = "ContinuousUniform";
            wQuality.FeedbackStd.Params = new double[] { 2, 2 };
            wQuality.QualityMean.Type = "ContinuousUniform";
            wQuality.QualityMean.Params = new double[] { 10, 10 };
            wQuality.QualityStd.Type = "ContinuousUniform";
            wQuality.QualityStd.Params = new double[] { 0.001, 0.001 };
            wQuality.ProcessingTimeMean.Type = "Normal";
            wQuality.ProcessingTimeMean.Params = new double[] { 300, 50};
            wQuality.ProcessingTimeStd.Type = "Normal";
            wQuality.ProcessingTimeStd.Params = new double[] { 300, 0.2};

            XmlUtils.Save(path, InputXmlShema.Default);
            var xml = XmlUtils.Load<InputXmlShema>(path);
            Assert.IsTrue(xml?.V != null);
        }


        [TestMethod()]
        public void LoadTest()
        {
            Assert.IsTrue(File.Exists(path));
            var xml = XmlUtils.Load<InputXmlShema>(path);
            Assert.IsTrue(xml?.V != null);
        }


        private void AddComments()
        {
            XmlUtils.AddXmlEntryDocumentation(path, "Executions",
                "Distribution Options: ContinuousUniform, Exponential, Laplace, Normal, StudentT etc. ");
            XmlUtils.AddXmlEntryDocumentation(path, "FeedbackMean",
                "Feedback is generated with normal distribution Normal(Mean, Std)");
            XmlUtils.AddXmlEntryDocumentation(path, "QualityMean",
                "Quality is generated with normal distribution Normal(Mean, Std)");
            XmlUtils.AddXmlEntryDocumentation(path, "ProcessingTimeMean",
                "ResponseTime is generated with normal distribution Normal(Mean, Std)");
        }
    }
}