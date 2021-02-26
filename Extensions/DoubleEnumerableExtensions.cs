using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KrebsCycleQueuesSimulation.Extensions
{
    public static class DoubleEnumerableExtensions
    {
        public static IEnumerable<double> Add(this IEnumerable<double> a, IEnumerable<double> b)
        {
            if (a.Count() != b.Count())
                throw new ArgumentException($"Dimensions not compatible. {a.Count()} != {b.Count()}");

            return a.Zip(b, (x, y) => x + y);
        }
        public static IEnumerable<double> ReduceSum(this IEnumerable<IEnumerable<double>> a)
        {
            return a.Aggregate((x, y) => x.Add(y));
        }
        public static IEnumerable<double> Multiply(this IEnumerable<double> a, double b) => a.Select(x => x * b);

        public static double[] Average(this IEnumerable<double[]> a)
        {
            return a.ReduceSum()
                .Multiply(1.0 / (double)a.Count())
                .ToArray();
        }
    }
}
