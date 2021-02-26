using KrebsCycleQueuesSimulation.Extensions;
using KrebsCycleQueuesSimulation.Structs;
using KrebsSimulationOptimized.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KrebsCycleQueuesSimulation.Functions
{
    public static class Simulation
    {
        public static IEnumerable<KrebsProducts> SimulateWithNoiseAndInhibition(
            KrebsProducts start_products, KrebsConstants constants,
            double noise_amplitude, KrebsRatesInhibitionConfiguration inhibition
            )
        {
            Random b = new Random();
            int time_to_change_noise_tables = 0;
            double[] noise_table = null;
            void change_noise_table()
            {
                double n() => 1.0 + noise_amplitude * b.NextGaussian();
                noise_table = new double[12];
                for (int i = 0; i < noise_table.Length; i++)
                    noise_table[i] = n();
            }
            change_noise_table();

            KrebsRates noise_and_inhibition(KrebsRates r)
            {
                time_to_change_noise_tables += 1;
                if (time_to_change_noise_tables == 100)
                {
                    time_to_change_noise_tables = 0;
                    change_noise_table();
                }
                var noise = r.ToArray().Zip(noise_table, (a, c) => a * c).ToArray();
                var rnoise = new KrebsRates(noise);
                //var rnoise = r.ApplyGaussianNoise(noise_amplitude);

                rnoise.Pyr_Accoa = (1.0 - inhibition.Pyr_Accoa) * rnoise.Pyr_Accoa;
                rnoise.Pyr_Oxo = (1.0 - inhibition.Pyr_Oxo) * rnoise.Pyr_Oxo;
                rnoise.AccoaOxo_Cit = (1.0 - inhibition.AccoaOxo_Cit) * rnoise.AccoaOxo_Cit;
                rnoise.Cit_Iso = (1.0 - inhibition.Cit_Iso) * rnoise.Cit_Iso;
                rnoise.Iso_Keto = (1.0 - inhibition.Iso_Keto) * rnoise.Iso_Keto;
                rnoise.Keto_Sca = (1.0 - inhibition.Keto_Sca) * rnoise.Keto_Sca;
                rnoise.Sca_Fum = (1.0 - inhibition.Sca_Fum) * rnoise.Sca_Fum;
                rnoise.Fum_Mal = (1.0 - inhibition.Fum_Mal) * rnoise.Fum_Mal;
                rnoise.Mal_Oxo = (1.0 - inhibition.Mal_Oxo) * rnoise.Mal_Oxo;
                rnoise.OxoBalance = (1.0 - inhibition.Oxo_Balance) * rnoise.OxoBalance;
                rnoise.CitBalance = (1.0 - inhibition.Cit_Balance) * rnoise.CitBalance;

                return rnoise;
            }

            return SimulateReduced(start_products, constants, noise_amplitude, noise_and_inhibition);
        }

        public static IEnumerable<KrebsProducts> SimulateReduced(KrebsProducts start_products,
            KrebsConstants constants,
            double noise_amplitude,
            Func<KrebsRates, KrebsRates> adjust_rates_fn = null
            )
        {
            var p = start_products;
            Random b = new Random();
            double noise = 0;

            while (true)
            {
                yield return p;
                for (int i = 0; i < 1000; i++)
                {
                    var rates = ComputeRates.ComputeReduced(p, constants);
                    if (adjust_rates_fn != null)
                        rates = adjust_rates_fn(rates);

                    p = ComputeQueues.ComputeReduced(p, rates);
                    if (i % 100 == 0)
                        noise = noise_amplitude * b.NextGaussian();

                    p.Pyr = start_products.Pyr * (1.0 + noise);
                }
            }
        }
    }
}
