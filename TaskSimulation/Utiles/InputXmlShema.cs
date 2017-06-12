namespace TaskSimulation.Utiles
{
    public class InputXmlShema
    {
        public Version V;

        public Execution[] Executions;

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public class Version
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int V { get; set; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public class Execution
        {
            /// <remarks/>
            public ExecutionTaskArrivalRate TaskArrivalRate { get; set; }

            /// <remarks/>
            public ExecutionWorkerArrivalRate WorkerArrivalRate { get; set; }

            /// <remarks/>
            public ExecutionWorkerLeaveRate WorkerLeaveRate { get; set; }

            /// <remarks/>
            public ExecutionWorkersQualityDistribution WorkersQualityDistribution { get; set; }

            /// <remarks/>
            public string GradeSystem { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public long InitialNumOfWorkers { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public double MaxSimulationTime { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public long Seed { get; set; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class ExecutionTaskArrivalRate
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public double[] Params { get; set; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class ExecutionWorkerArrivalRate
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public double[] Params { get; set; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class ExecutionWorkerLeaveRate
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public double[] Params { get; set; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class ExecutionWorkersQualityDistribution
        {
            /// <remarks/>
            public ExecutionWorkersQualityDistributionFeedbackMean FeedbackMean { get; set; }

            /// <remarks/>
            public ExecutionWorkersQualityDistributionFeedbackStd FeedbackStd { get; set; }

            /// <remarks/>
            public ExecutionWorkersQualityDistributionQualityMean QualityMean { get; set; }

            /// <remarks/>
            public ExecutionWorkersQualityDistributionQualityStd QualityStd { get; set; }

            /// <remarks/>
            public ExecutionWorkersQualityDistributionResponseTimeMean ProcessingTimeMean { get; set; }

            /// <remarks/>
            public ExecutionWorkersQualityDistributionResponseTimeStd ProcessingTimeStd { get; set; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class ExecutionWorkersQualityDistributionFeedbackMean
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public double[] Params { get; set; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class ExecutionWorkersQualityDistributionFeedbackStd
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public double[] Params { get; set; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class ExecutionWorkersQualityDistributionQualityMean
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public double[] Params { get; set; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class ExecutionWorkersQualityDistributionQualityStd
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public double[] Params { get; set; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class ExecutionWorkersQualityDistributionResponseTimeMean
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public double[] Params { get; set; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class ExecutionWorkersQualityDistributionResponseTimeStd
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public double[] Params { get; set; }
        }
        
        public static readonly InputXmlShema Default = new InputXmlShema()
        {
            V = new Version() { V = 1 },
            Executions = new Execution[]
            {
                new Execution()
                {
                    InitialNumOfWorkers = 10,
                    MaxSimulationTime = 10,
                    Seed = 1,
                    TaskArrivalRate = new ExecutionTaskArrivalRate()
                    {
                        Type = "ContinuousUniform",
                        Params = new double[] {1, 2}
                    },
                    WorkerArrivalRate = new ExecutionWorkerArrivalRate()
                    {
                        Type = "ContinuousUniform",
                        Params = new double[] {1, 2}
                    },
                    WorkerLeaveRate = new ExecutionWorkerLeaveRate()
                    {
                        Type = "ContinuousUniform",
                        Params = new double[] {1, 2}
                    },

                    WorkersQualityDistribution = new ExecutionWorkersQualityDistribution()
                    {
                        FeedbackMean = new ExecutionWorkersQualityDistributionFeedbackMean()
                        {
                            Type = "ContinuousUniform",
                            Params = new double[] {1, 2}
                        },
                        FeedbackStd = new ExecutionWorkersQualityDistributionFeedbackStd()
                        {
                            Type = "ContinuousUniform",
                            Params = new double[] {1, 2}
                        },
                        QualityMean = new ExecutionWorkersQualityDistributionQualityMean()
                        {
                            Type = "ContinuousUniform",
                            Params = new double[] {1, 2}
                        },
                        QualityStd = new ExecutionWorkersQualityDistributionQualityStd()
                        {
                            Type = "ContinuousUniform",
                            Params = new double[] {1, 2}
                        },
                        ProcessingTimeMean = new ExecutionWorkersQualityDistributionResponseTimeMean()
                        {
                            Type = "ContinuousUniform",
                            Params = new double[] {1, 2}
                        },
                        ProcessingTimeStd = new ExecutionWorkersQualityDistributionResponseTimeStd()
                        {
                            Type = "ContinuousUniform",
                            Params = new double[] {1, 2}
                        },
                    }
                }
            }
        };
    }
}
