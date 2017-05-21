using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;

namespace TaskSimulation.Distribution
{
    public class ReflectIContinuousDistribution
    {
        public static IContinuousDistribution GetDistribution(string distributionClassName, params object[] param)
        {
            var assembly = Assembly.Load("MathNet.Numerics");
            var type = assembly.GetType("MathNet.Numerics.Distributions." + distributionClassName);

            if (type == null) return null;

            var result = Activator.CreateInstance(type, param);

            var continuousDistribution = (result as IContinuousDistribution);

            return continuousDistribution;
        }

        public static IContinuousDistribution GetDistribution(string distributionClassName, object[] param, Random random)
        {
            var paramList = param.ToList();
            paramList.Add(random);

            return GetDistribution(distributionClassName, paramList.ToArray());
        }
        
        public static IContinuousDistribution GetDistribution(string distributionClassName, double[] param, Random random)
        {
            return GetDistribution(distributionClassName, param.Cast<object>().ToArray(), random);
        }
    }
}
