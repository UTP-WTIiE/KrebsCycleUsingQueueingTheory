using KrebsCycleQueuesSimulation.Structs;
using KrebsSimulationOptimized.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KrebsCycleQueuesSimulation.Functions
{
    public static class Perform
    {
        public static void PerformSimulation(
            string path_to_output_folder,
            KrebsProducts products,
            KrebsConstants constants,
            double noise_amplitude,
            KrebsRatesInhibitionConfiguration inhibition,
            int iterations,
            int seconds
        )
        {
            var simulation_results = Experiment.Conduct(
                iterations: iterations,
                steps: seconds,
                iteration_fn: () =>
                {
                    return Simulation.SimulateWithNoiseAndInhibition(products, constants, noise_amplitude, inhibition);
                }
            );

            int iteration = 0;
            path_to_output_folder = Path.Combine(path_to_output_folder, Guid.NewGuid().ToString());
            Directory.CreateDirectory(path_to_output_folder);

            foreach (var result in simulation_results)
            {
                string path = Path.Combine(path_to_output_folder, $"iter_{iteration}.csv");
                GenerateCsv.SaveKrebsReducedSimulationResults(
                    path,
                    result,
                    constants,
                    (r) =>
                    {
                        var rnoise = r.ApplyGaussianNoise(noise_amplitude);
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
                    );
                iteration++;
            }
        }
    }
}
