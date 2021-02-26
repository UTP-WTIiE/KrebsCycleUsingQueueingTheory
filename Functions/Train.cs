using KrebsCycleQueuesSimulation.Extensions;
using KrebsCycleQueuesSimulation.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KrebsCycleQueuesSimulation.Functions
{
    public static class Train
    {
        public static void TrainSimulation(
            string output_folder_path,
            KrebsProducts start_products,
            int seconds,
            double noise_amplitude,
            int chromosomes_count,
            int chromosomes_for_reproduction,
            double mutation_chance = 0.1,
            double mutation_magnitude = 1.0,
            List<KrebsConstants> start_population = null
            )
        {
            var initialization_chromosomes = new List<double[]>();
            if (start_population != null)
            {
                for (int i = 0; i < chromosomes_count; i++)
                {
                    var array = new double[start_population[0].ToArray().Length];
                    start_population[i % start_population.Count].ToArray().CopyTo(array, 0);
                    initialization_chromosomes.Add(array);
                }
            }

            Func<KrebsConstants, double> fitness_fn = (KrebsConstants constants) => FitnessFunction.EvaluateConstantsReduced(
                startProducts: start_products,
                constants: constants,
                noise_amplitude: noise_amplitude,
                seconds_to_simulate: seconds,
                adjustRates: null,
                relative_loss: true
                );
            Action<List<(double[] chromosome, double eval_value)>> on_epoch_end = (chromosomes) =>
            {
                var save_data = chromosomes.Select(x => x.Item1).To2DArray();
                string output_path = Path.Combine(output_folder_path, $"{chromosomes.Min(x => x.Item2)}.csv");
                GeneticAlgorithm.SaveBestGenomes(chromosomes, output_folder_path, header: new KrebsConstants(chromosomes[0].chromosome).GetCsvHeader());
                GeneticAlgorithm.PrintBestScores(chromosomes);
            };

            double lower_bound = 0.01;
            double upper_bound = 0.1;
            Func<double[], int, double> eval_reproduction_fn = (chromosome, packet) =>
            {
                var below_zero_loss = chromosome
                .Select(x => Math.Min(x, 0))
                .Select(x => Math.Abs(x))
                .Sum();

                var rate = ComputeRates.ComputeRateReduced(chromosome, start_products, packet);

                var below_margin = packet < 9 ? Math.Abs(Math.Min(rate - lower_bound, 0)) : 0;
                var above_margin = Math.Abs(Math.Max(rate - upper_bound, 0));

                return below_zero_loss + below_margin + above_margin;
            };

            Console.WriteLine("Start algorithm...");
            GeneticAlgorithm.Perform(
                fitness_function: fitness_fn,
                chromosome_length: KrebsConstants.ConstsCount,
                iterations: 1000,
                chromosomes_count: chromosomes_count,
                chromosomes_for_reproduction: chromosomes_for_reproduction,
                mutation_chance: mutation_chance,
                mutation_magnitude: mutation_magnitude,
                on_epoch_end_fn: on_epoch_end,
                reproduction_evaluation_fn: eval_reproduction_fn,
                initial_chromosomes: initialization_chromosomes,
                create_chromosome_packets_fn: (array) => new KrebsConstants(array)
                );
        }
    }
}
