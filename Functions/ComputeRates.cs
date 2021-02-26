using KrebsCycleQueuesSimulation.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace KrebsCycleQueuesSimulation.Functions
{
    public static class ComputeRates
    {
        public static KrebsRates ComputeReduced(KrebsProducts products, KrebsConstants c)
        {
            KrebsRates r = new KrebsRates();

            r.Pyr_Accoa = _rate1_1(products.Pyr, products.Accoa, c.Pyr_AccoaConsts);
            r.Pyr_Oxo = _pyr_oxo_rate(products.Pyr, products.Oxo, c.Pyr_OxoConsts);
            r.AccoaOxo_Cit = _accoaOxo_cit_rate(products.Accoa, products.Oxo, products.Cit, c.AccoaOxo_CitConsts);

            r.Cit_Iso = _rate1_1(products.Cit, products.Iso * KrebsProducts.cis_aco_ratio, c.Cit_IsoConsts);
            r.Iso_Keto = _rate1_1(products.Iso * KrebsProducts.iso_ratio, products.Keto, c.Iso_KetoConsts);
            r.Keto_Sca = _rate1_1(products.Keto, products.Sca * KrebsProducts.sca_ratio, c.Keto_ScaConsts);
            r.Sca_Fum = _rate1_1(products.Sca * KrebsProducts.suc_ratio, products.Fum, c.Sca_FumConsts);
            r.Fum_Mal = _rate1_1(products.Fum, products.Mal, c.Fum_MalConsts);
            r.Mal_Oxo = _rate1_1(products.Mal, products.Oxo, c.Mal_OxoConsts);

            r.OxoBalance = _balance_rate(products.Oxo, c.Oxo_Balance);
            r.CitBalance = _balance_rate(products.Cit, c.Cit_Balance);

            return r;
        }

        public static double ComputeRateReduced(double[] c, KrebsProducts products, int rate)
        {
            double r = 0;
            switch (rate)
            {
                case 0: r = _rate1_1(products.Pyr, products.Accoa, c); break;
                case 1: r = _pyr_oxo_rate(products.Pyr, products.Oxo, c); break;
                case 2: r = _accoaOxo_cit_rate(products.Accoa, products.Oxo, products.Cit, c); break;
                case 3: r = _rate1_1(products.Cit, products.Iso * KrebsProducts.cis_aco_ratio, c); break;
                case 4: r = _rate1_1(products.Iso * KrebsProducts.iso_ratio, products.Keto, c); break;
                case 5: r = _rate1_1(products.Keto, products.Sca * KrebsProducts.sca_ratio, c); break;
                case 6: r = _rate1_1(products.Sca * KrebsProducts.suc_ratio, products.Fum, c); break;
                case 7: r = _rate1_1(products.Fum, products.Mal, c); break;
                case 8: r = _rate1_1(products.Mal, products.Oxo, c); break;
                case 9: r = _balance_rate(products.Oxo, c); break;
                case 10: r = _balance_rate(products.Cit, c); break;
                default:
                    throw new Exception($"Unknown rate: {rate}");
            }
            return r;
        }

        public static double _rate1_1(double x1, double x2, double[] c)
        {
            if (c[1] == 0 || c[3] == 0)
                return 0;

            double vf = c[0];
            double ks1 = c[1];
            double vr = c[2];
            double kp1 = c[3];

            double l1 = x1 * vf / ks1;
            double l2 = x2 * vr / kp1;
            double m = 1 + x1 / ks1 + x2 / kp1;

            var r = (l1 - l2) / m;
            return r;
        }

        public static double _pyr_oxo_rate(double pyr, double oxo, double[] _c)
        {
            var a = pyr;
            var b = 0.003;
            var c = 0.0159;
            var d = oxo;
            var e = 0.0937;
            var f = 0.05;
            var g = _c[0];
            var h = _c[1];
            var i = _c[2];
            var j = _c[3];
            var g2 = _c[4];
            var k = _c[5];
            var l = _c[6];
            var m = _c[7];

            var l1 = g * a * b * c / h / i / j;
            var l2 = g2 * d * e * f / k / l / m;
            var m1 = 1.0 + a / h + d / k;
            var m2 = 1.0 + b / i + e / l;
            var m3 = 1.0 + c / j + f / m;

            var r = (l1 - l2) / m1 / m2 / m3;
            return r;
        }

        public static double _accoaOxo_cit_rate(double s1, double s2, double p1, double[] c)
        {
            double vf = c[0];
            double ks1 = c[1];
            double ks2 = c[2];
            double vr = c[3];
            double kp1 = c[4];
            double kp2 = c[5];

            double l1 = vf * s1 * s2 / ks1 / ks2;
            double l2 = vr * p1 * 0.044 / kp1 / kp2;
            double m1 = 1.0 + s1 / kp1 + p1 / kp1;
            double m2 = 1.0 + s2 / kp2 + 0.044 / kp2;

            var r = (l1 - l2) / m1 / m2;
            return r;
        }

        public static double _balance_rate(double x1, double[] c)
        {
            return x1 * c[0];// + c[1];
        }
    }
}
