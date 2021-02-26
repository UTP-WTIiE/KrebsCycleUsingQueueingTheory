using KrebsCycleQueuesSimulation.Extensions;
using KrebsCycleQueuesSimulation.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KrebsCycleQueuesSimulation.Functions
{
    public static class GenerateCsv
    {
        public static void SaveKrebsReducedSimulationResults(string output_path, IEnumerable<KrebsProducts> products, KrebsConstants constants, Func<KrebsRates, KrebsRates> adjust_rates_fn = null, Func<KrebsProducts, KrebsProducts> normalize_fn = null)
        {
            string header = "PYR;ACCOA;OXO;CIT;ISO;KETO;SCA;FUM;MAL;PYR -> ACCOA; PYR -> OXO; ACCOA + OXO -> CIT; CIT -> ISO; ISO -> KETO; KETO -> SCA; SCA -> FUM; FUM -> MAL; MAL -> OXO;OXO BALANCE; CIT BALANCE";

            double[] get(KrebsProducts p)
            {
                var _p = normalize_fn != null ? normalize_fn(p) : p;
                var rates = ComputeRates.ComputeReduced(_p, constants);
                if (adjust_rates_fn != null)
                    rates = adjust_rates_fn(rates);

                return p.ToArray().Concat(rates.ToArray()).ToArray();
            }

            var data = products
                .Select(x => get(x))
                .To2DArray();

            Save(data, output_path, header);
        }

        public static void Save(double[,] data, string output_path, string header)
        {
            using (var writer = new StreamWriter(output_path))
            {
                var iLength = data.GetLength(0);
                var jLength = data.GetLength(1);

                writer.Write(header);
                writer.WriteLine();
                for (int i = 0; i < iLength; i++)
                {
                    for (int j = 0; j < jLength; j++)
                    {
                        writer.Write("{0};", data[i, j].ToString().Replace(',', '.'));
                    }

                    writer.WriteLine();
                }
            }
        }
    }
}
