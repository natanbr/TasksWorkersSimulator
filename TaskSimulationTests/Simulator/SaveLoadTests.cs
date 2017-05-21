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

            XmlUtils.Save(path, shema);
            XmlUtils.AddXmlEntryDocumentation(path, "Executions",
                "Distribution Options: ContinuousUniform, Exponential, Laplace, Normal, StudentT etc. ");
            XmlUtils.AddXmlEntryDocumentation(path, "FeedbackMean",
                "Feedback is generated with normal distribution Normal(Mean, Std)");
            XmlUtils.AddXmlEntryDocumentation(path, "QualityMean",
                "Quality is generated with normal distribution Normal(Mean, Std)");
            XmlUtils.AddXmlEntryDocumentation(path, "ResponseTimeMean",
                "ResponseTime is generated with normal distribution Normal(Mean, Std)");
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
        public void LoadTest()
        {
            Assert.IsTrue(File.Exists(path));
            var xml = XmlUtils.Load<InputXmlShema>(path);
            Assert.IsTrue(xml?.V != null);
        }
    }
}