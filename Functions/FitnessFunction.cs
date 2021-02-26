using KrebsCycleQueuesSimulation.Extensions;
using KrebsCycleQueuesSimulation.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KrebsCycleQueuesSimulation.Functions
{
    public static class FitnessFunction
    {
        public static double EvaluateConstantsReduced(
            KrebsProducts startProducts,
            KrebsConstants constants,
            double noise_amplitude,
            int seconds_to_simulate,
            Func<KrebsRates, KrebsRates> adjustRates = null,
            bool relative_loss = true
            )
        {
            var simulation = Simulation.SimulateReduced(
                startProducts,
                constants,
                noise_amplitude,
                adjustRates
                )
                .Take(seconds_to_simulate)
                .ToList();

            var simres = simulation
                .Skip(seconds_to_simulate - 100)
                .Take(100)
                .Select(x => x.ToArray())
                .Average();

            var end_products = new KrebsProducts(simres);

            double loss = 0;
            if (relative_loss)
            {
                var losses = end_products.ToArray()
                    .Zip(startProducts.ToArray(), (a, b) => Math.Abs(a - b) / b);

                loss = losses.Sum();
            }
            else
            {
                loss = startProducts.ToArray()
                    .Zip(end_products.ToArray(), (a, b) => Math.Pow(a - b, 2))
                    .Sum();
                loss = loss / 0.0001;
            }

            return loss;
        }

    }
}
