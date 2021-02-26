using KrebsCycleQueuesSimulation.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace KrebsCycleQueuesSimulation.Functions
{
    public static class ComputeQueues
    {
        static double delta = 0.0001;

        public static KrebsProducts ComputeReduced(KrebsProducts p, KrebsRates r)
        {
            double r0 = _queue_non_reversable(r.Pyr_Accoa, p.Pyr);
            double r1 = _queue_non_reversable(r.Pyr_Oxo, p.Pyr);
            double r2 = _queue_non_reversable(r.AccoaOxo_Cit, p.Accoa, p.Oxo);
            double r3 = _queue_non_reversable(r.Cit_Iso, p.Cit);
            double r4 = _queue_non_reversable(r.Iso_Keto, p.Iso);
            double r5 = _queue_non_reversable(r.Keto_Sca, p.Keto);
            double r6 = _queue_non_reversable(r.Sca_Fum, p.Sca);
            double r7 = _queue_non_reversable(r.Fum_Mal, p.Fum);
            double r8 = _queue_non_reversable(r.Mal_Oxo, p.Mal);

            double b2 = _queue_non_reversable(r.OxoBalance, p.Oxo);
            double b3 = _queue_non_reversable(r.CitBalance, p.Cit);

            KrebsProducts c = new KrebsProducts();
            c.Pyr = -1 * r0 - r1;
            c.Accoa = r0 - r2;
            c.Oxo = r1 - r2 + r8 - b2;
            c.Cit = r2 - r3 - b3;
            c.Iso = r3 - r4;
            c.Keto = r4 - r5;
            c.Sca = r5 - r6;
            c.Fum = r6 - r7;
            c.Mal = r7 - r8;

            return p + c;
        }

        public static double _queue_non_reversable(double rate, double q1)
        {
            if (r(rate) && q1 > delta)
                return delta;
            else
                return 0;
        }

        public static double _queue_non_reversable(double rate, double q1, double q2)
        {
            if (r(rate) && q1 > delta && q2 > delta)
                return delta;
            else
                return 0;
        }

        private static bool r(double rate)
        {
            return new Random().NextDouble() < rate;
        }
    }
}
