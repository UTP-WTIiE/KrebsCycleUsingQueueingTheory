using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KrebsCycleQueuesSimulation.Functions
{
    public static class Experiment
    {
        public static List<List<T>> Conduct<T>(int iterations, int steps, Func<IEnumerable<T>> iteration_fn)
        {
            List<IEnumerable<T>> experiments = new List<IEnumerable<T>>();

            for (int i = 0; i < iterations; i++)
            {
                var result = iteration_fn().Take(steps);
                experiments.Add(result);
            }

            return experiments
                .AsParallel()
                .Select(x => x.ToList())
                .ToList();
        }
    }
}
