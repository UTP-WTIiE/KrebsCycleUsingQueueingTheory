using KrebsCycleQueuesSimulation.Extensions;
using KrebsCycleQueuesSimulation.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KrebsCycleQueuesSimulation.Functions
{
    public static class GeneticAlgorithm
    {
        public static List<(double[], double)> Perform(
            Func<KrebsConstants, double> fitness_function,
            int chromosome_length,
            int iterations = 100,
            int chromosomes_count = 100,
            int chromosomes_for_reproduction = 10,
            double mutation_chance = 0.1,
            double mutation_magnitude = 1.0,
            Action<List<(double[] chromosome, double eval_value)>> on_epoch_end_fn = null,
            Func<double[], int, double> reproduction_evaluation_fn = null,
            List<double[]> initial_chromosomes = null,
            Func<double[], KrebsConstants> create_chromosome_packets_fn = null
        )
        {
            Random b = new Random();
            List<double[]> chromosomes = initial_chromosomes;
            if (chromosomes == null)
                chromosomes = new List<double[]>();

            if (chromosomes.Count < chromosomes_count)
            {
                for (int i = chromosomes.Count; i < chromosomes_count; i++)
                {
                    double[] chr = new double[chromosome_length];
                    for (int j = 0; j < chr.Length; j++)
                        chr[j] = 0.2 * b.NextDouble();

                    chromosomes.Add(chr);
                }
            }

            List<(double[], double)> reproductive_chromosomes = new List<(double[], double)>();

            for (int i = 0; i < iterations; i++)
            {
                Console.WriteLine($"{DateTime.Now.ToShortTimeString()} Evaluating...");
                // evaluate chromosomes
                reproductive_chromosomes = chromosomes
                    .AsParallel()
                    .Select(x => (x, fitness_function(new KrebsConstants(x))))
                    .OrderBy(x => x.Item2)
                    .Take(chromosomes_for_reproduction)
                    .ToList();

                double[] rand_pick() => reproductive_chromosomes[b.Next(reproductive_chromosomes.Count)].Item1;

                // pass to list
                chromosomes = new List<double[]>();
                reproductive_chromosomes.ForEach(x => chromosomes.Add(x.Item1));

                Console.WriteLine($"{DateTime.Now.ToShortTimeString()} Reproducing...");
                // reproduce
                for (int j = reproductive_chromosomes.Count; j < chromosomes_count; j++)
                {
                    var _x1 = create_chromosome_packets_fn == null ? new KrebsConstants(rand_pick()) : create_chromosome_packets_fn(rand_pick());
                    var _x2 = create_chromosome_packets_fn == null ? new KrebsConstants(rand_pick()) : create_chromosome_packets_fn(rand_pick());
                    List<double[]> reproduced_packets = new List<double[]>();
                    for (int packet = 0; packet < _x1.ToPacketsList().Count; packet++)
                    {
                        double[] x1 = _x1.ToPacketsList()[packet];
                        double[] x2 = _x2.ToPacketsList()[packet];
                        Random r = new Random();
                        while (true)
                        {
                            var x = r.NextDouble() < 0.5 ? x1 : x2;

                            
                            x = x2.Select(_x => r.NextDouble() < mutation_chance ? _x * (1.0 + mutation_magnitude * (r.NextDouble() - 0.5)) : _x)
                                .ToArray();
                            if (reproduction_evaluation_fn == null)
                            {
                                reproduced_packets.Add(x);
                                break;
                            }
                            else
                            {
                                double x_score = reproduction_evaluation_fn(x, packet);
                                if (x_score <= 1e-4)
                                {
                                    reproduced_packets.Add(x);
                                    break;
                                }
                                else
                                {
                                    double x1_score = reproduction_evaluation_fn(x1, packet);
                                    double x2_score = reproduction_evaluation_fn(x2, packet);

                                    if (x_score < x1_score)
                                        x1 = x;
                                    if (x_score < x2_score)
                                        x2 = x;
                                }
                            }
                        }

                    }
                    var new_chromosome = reproduced_packets
                        .SelectMany(x => x)
                        .ToArray();

                    chromosomes.Add(new_chromosome);
                }

                on_epoch_end_fn?.Invoke(reproductive_chromosomes);
            }

            return reproductive_chromosomes;
        }

        public static double[] Reproduce(double[] x1, double[] x2, double mutation_chance, double mutation_magnitude)
        {
            Random r = new Random();
            List<double[]> x1_bundles = new List<double[]>();
            List<double[]> x2_bundles = new List<double[]>();
            for (int i = 0; i < 11; i++)
            {
                x1_bundles.Add(x1.Skip(i * 4).Take(4).ToArray());
                x2_bundles.Add(x2.Skip(i * 4).Take(4).ToArray());
            }
            for (int i = 44; i < 47; i++)
            {
                x1_bundles.Add(new double[] { x1[i] });
                x2_bundles.Add(new double[] { x2[i] });
            }
            return x1_bundles
                .Zip(x2_bundles, (a, b) => r.NextDouble() < 0.5 ? a : b)
                .SelectMany(x => x)
                .Select(x => r.NextDouble() < mutation_chance
                    ? x * (1 + mutation_magnitude * r.NextGaussian()) + 0.0001 * (r.Next(0, 3) - 1)
                    : x)
                .Select(x => Math.Abs(x))
                .ToArray();
            //return x1
            //    .Zip(x2, (a, b) => r.NextDouble() < 0.5 ? a : b)
            //    .Select(x => r.NextDouble() < mutation_chance 
            //        ? x * (1 + mutation_magnitude * r.NextGaussian()) + 0.0001 * (r.Next(0,3) - 1) 
            //        : x)
            //    .Select(x=> Math.Abs(x))
            //    .ToArray();
        }

        public static void SaveBestGenomes(List<(double[], double)> chromosomes, string output_folder_path, string additional_name = null, string header = null)
        {
            header = header ?? "PYR;ACCOA;OXO;CIT;CIT_ACO;ISO;KETO;SCA;SUC;FUM;MAL";
            var data = chromosomes.Select(x => x.Item1).To2DArray();
            string output_path = Path.Combine(output_folder_path, $"{additional_name ?? string.Empty}_{chromosomes.Min(x => x.Item2)}.csv");
            GenerateCsv.Save(data, output_path, header);
        }

        public static void PrintBestScores(List<(double[], double)> chromosomes)
        {
            Console.WriteLine("----------------------");
            chromosomes.ForEach(x => Console.WriteLine(x.Item2));
        }
    }
}
