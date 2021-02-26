using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KrebsCycleQueuesSimulation.Structs
{
    public class KrebsConstants
    {
        public static KrebsConstants DefaultConstants()
        {
            return new KrebsConstants(new double[ConstsCount]
            {
                //PYR -> AcCoa
                0.048599358,
                0.006040764,
                0.000522196,
                0.068117692,
                //Pyr -> Oxo
                488.7994860651200,
                0.1953179695507,
                0.8486246578185,
                0.0760822325802,
                9.4718631339845,
                2.0692053883955,
                17.0742721650473,
                0.5347311584764,
                //AcCoa + Oxo -> Cit
                0.051737638,
                0.046112657,
                0.050474047,
                0.000275255,
                0.531448578,
                0.484991908,
                // Cit -> Iso
                0.045569308,
                0.547527339,
                0.000550254,
                1.02452366,
                // Iso -> Keto
                0.041579151,
                0.006179927,
                0.00047688,
                0.089210344,
                // Keto -> Sca
                0.015858709,
                0.137667467,
                0.000244722,
                3.260889572,
                // Sca -> Fum
                0.0217351,
                0.026234502,
                0.000166406,
                0.551239235,
                // Fum -> Mal
                0.072196231,
                0.10545549,
                0.000425816,
                0.023982491,
                // Mal -> Oxo
                0.186864995,
                6.191970322,
                0.002474724,
                0.056927648,
                // Oxo Balance
                8.379245689,
                // Cit Balance
                0.068549709
            });
        }

        public const int ConstsCount = 44;
        private double[] consts;
        public const double reverse_current_factor = 0.01;

        public KrebsConstants(double[] consts)
        {
            if (consts.Length != ConstsCount)
                throw new ArgumentException($"Array should have {ConstsCount} length instead of {consts.Length}");
            this.consts = new double[consts.Length];
            consts.CopyTo(this.consts, 0);
        }
        private double[] c(int start, int length) => this.consts.Skip(start).Take(length).ToArray();

        public double[] Pyr_AccoaConsts => c(0, 4);

        public double[] Pyr_OxoConsts => c(4, 8);

        public double[] AccoaOxo_CitConsts => c(12, 6);

        public double[] Cit_IsoConsts => c(18, 4);

        public double[] Iso_KetoConsts => c(22, 4);

        public double[] Keto_ScaConsts => c(26, 4);

        public double[] Sca_FumConsts => c(30, 4);

        public double[] Fum_MalConsts => c(34, 4);

        public double[] Mal_OxoConsts => c(38, 4);

        public double[] Oxo_Balance => c(42, 1);

        public double[] Cit_Balance => c(43, 1);

        private double[] copy(double[] c)
        {
            var a = new double[c.Length];
            c.CopyTo(a, 0);
            return a;
        }

        public double[] ToArray()
        {
            return copy(this.consts);
        }

        public List<double[]> ToPacketsList()
        {
            return new List<double[]>()
            {
                copy(Pyr_AccoaConsts),
                copy(Pyr_OxoConsts),
                copy(AccoaOxo_CitConsts),
                copy(Cit_IsoConsts),
                copy(Iso_KetoConsts),
                copy(Keto_ScaConsts),
                copy(Sca_FumConsts),
                copy(Fum_MalConsts),
                copy(Mal_OxoConsts),
                copy(Oxo_Balance),
                copy(Cit_Balance)
            };
        }

        public string GetCsvHeader()
        {
            string header = "";
            void s(double[] a, string name)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    header += $"{name}_{i};";
                }
            }

            s(Pyr_AccoaConsts, nameof(Pyr_AccoaConsts));
            s(Pyr_OxoConsts, nameof(Pyr_OxoConsts));
            s(AccoaOxo_CitConsts, nameof(AccoaOxo_CitConsts));
            s(Cit_IsoConsts, nameof(Cit_IsoConsts));
            s(Iso_KetoConsts, nameof(Iso_KetoConsts));
            s(Keto_ScaConsts, nameof(Keto_ScaConsts));
            s(Sca_FumConsts, nameof(Sca_FumConsts));
            s(Fum_MalConsts, nameof(Fum_MalConsts));
            s(Mal_OxoConsts, nameof(Mal_OxoConsts));
            s(Oxo_Balance, nameof(Oxo_Balance));
            s(Cit_Balance, nameof(Cit_Balance));

            return header;
        }
    }
}
