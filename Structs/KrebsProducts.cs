using System;
using System.Collections.Generic;
using System.Text;

namespace KrebsCycleQueuesSimulation.Structs
{
    public struct KrebsProducts
    {
        public static KrebsProducts Default()
        {
            return new KrebsProducts(
                0.0586,
                0.5,
                0.006,
                0.19,
                0.02,
                0.54,
                0.73,
                0.485,
                0.495
                );
        }

        public double Pyr;
        public double Accoa;
        public double Oxo;
        public double Cit;
        public double Iso;
        public double Keto;
        public double Sca;
        public double Fum;
        public double Mal;

        public const double cis_aco_ratio = 1.0 / 13.5;
        public const double iso_ratio = 12.5 / 13.5;
        public const double sca_ratio = 0.66 / (0.66 + 0.07);
        public const double suc_ratio = 0.07 / (0.66 + 0.07);

        public static KrebsProducts operator +(KrebsProducts a, KrebsProducts b)
        {
            KrebsProducts c = new KrebsProducts();
            c.Pyr = a.Pyr + b.Pyr;
            c.Accoa = a.Accoa + b.Accoa;
            c.Oxo = a.Oxo + b.Oxo;
            c.Cit = a.Cit + b.Cit;
            c.Iso = a.Iso + b.Iso;
            c.Keto = a.Keto + b.Keto;
            c.Sca = a.Sca + b.Sca;
            c.Fum = a.Fum + b.Fum;
            c.Mal = a.Mal + b.Mal;
            return c;
        }

        public static KrebsProducts operator -(KrebsProducts a, KrebsProducts b)
        {
            KrebsProducts c = new KrebsProducts();
            c.Pyr = a.Pyr - b.Pyr;
            c.Accoa = a.Accoa - b.Accoa;
            c.Oxo = a.Oxo - b.Oxo;
            c.Cit = a.Cit - b.Cit;
            c.Iso = a.Iso - b.Iso;
            c.Keto = a.Keto - b.Keto;
            c.Sca = a.Sca - b.Sca;
            c.Fum = a.Fum - b.Fum;
            c.Mal = a.Mal - b.Mal;
            return c;
        }

        public static KrebsProducts operator *(KrebsProducts a, double b)
        {
            KrebsProducts c = new KrebsProducts();
            c.Pyr = a.Pyr * b;
            c.Accoa = a.Accoa * b;
            c.Oxo = a.Oxo * b;
            c.Cit = a.Cit * b;
            c.Iso = a.Iso * b;
            c.Keto = a.Keto * b;
            c.Sca = a.Sca * b;
            c.Fum = a.Fum * b;
            c.Mal = a.Mal * b;
            return c;
        }

        public static KrebsProducts operator /(KrebsProducts a, KrebsProducts b)
        {
            KrebsProducts c = new KrebsProducts();
            c.Pyr = a.Pyr / b.Pyr;
            c.Accoa = a.Accoa / b.Accoa;
            c.Oxo = a.Oxo / b.Oxo;
            c.Cit = a.Cit / b.Cit;
            c.Iso = a.Iso / b.Iso;
            c.Keto = a.Keto / b.Keto;
            c.Sca = a.Sca / b.Sca;
            c.Fum = a.Fum / b.Fum;
            c.Mal = a.Mal / b.Mal;
            return c;
        }

        public static KrebsProducts operator *(KrebsProducts a, KrebsProducts b)
        {
            KrebsProducts c = new KrebsProducts();
            c.Pyr = a.Pyr * b.Pyr;
            c.Accoa = a.Accoa * b.Accoa;
            c.Oxo = a.Oxo * b.Oxo;
            c.Cit = a.Cit * b.Cit;
            c.Iso = a.Iso * b.Iso;
            c.Keto = a.Keto * b.Keto;
            c.Sca = a.Sca * b.Sca;
            c.Fum = a.Fum * b.Fum;
            c.Mal = a.Mal * b.Mal;
            return c;
        }

        public KrebsProducts(double pyr, double accoa, double oxo, double cit, double iso, double keto, double sca, double fum, double mal)
        {
            Pyr = pyr;
            Accoa = accoa;
            Oxo = oxo;
            Cit = cit;
            Iso = iso;
            Keto = keto;
            Sca = sca;
            Fum = fum;
            Mal = mal;
        }

        public KrebsProducts(double[] array)
        {
            if (array.Length < 9)
                throw new ArgumentException();

            Pyr = array[0];
            Accoa = array[1];
            Oxo = array[2];
            Cit = array[3];
            Iso = array[4];
            Keto = array[5];
            Sca = array[6];
            Fum = array[7];
            Mal = array[8];
        }

        public double[] ToArray()
        {
            return new double[]
            {
                Pyr, Accoa, Oxo, Cit, Iso, Keto, Sca, Fum, Mal
            };
        }
    }
}
