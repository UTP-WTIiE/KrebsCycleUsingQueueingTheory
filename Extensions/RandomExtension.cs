using System;
using System.Collections.Generic;
using System.Text;

namespace KrebsCycleQueuesSimulation.Extensions
{
    public static class RandomExtension
    {
        public static double NextGaussian(this Random rand)
        {
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)

            return randStdNormal;
        }

    }
}
