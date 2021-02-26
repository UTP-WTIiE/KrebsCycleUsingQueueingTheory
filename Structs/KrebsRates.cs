using KrebsCycleQueuesSimulation.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace KrebsCycleQueuesSimulation.Structs
{
    public struct KrebsRates
    {
        public double Pyr_Accoa;
        public double Pyr_Oxo;
        public double AccoaOxo_Cit;
        public double Cit_Iso;
        public double Iso_Keto;
        public double Keto_Sca;
        public double Sca_Fum;
        public double Fum_Mal;
        public double Mal_Oxo;
        public double OxoBalance;
        public double CitBalance;

        public KrebsRates(double pyr_Accoa, double pyr_Oxo, double accoaOxo_Cit, double cit_iso, double iso_Keto, double keto_Sca, double sca_fum, double fum_Mal, double mal_Oxo, double oxo_balance, double cit_balance)
        {
            Pyr_Accoa = pyr_Accoa;
            Pyr_Oxo = pyr_Oxo;
            AccoaOxo_Cit = accoaOxo_Cit;
            Cit_Iso = cit_iso;
            Iso_Keto = iso_Keto;
            Keto_Sca = keto_Sca;
            Sca_Fum = sca_fum;
            Fum_Mal = fum_Mal;
            Mal_Oxo = mal_Oxo;
            OxoBalance = oxo_balance;
            CitBalance = cit_balance;
        }

        public KrebsRates(double[] array)
        {
            Pyr_Accoa = array[0];
            Pyr_Oxo = array[1];
            AccoaOxo_Cit = array[2];
            Cit_Iso = array[3];
            Iso_Keto = array[4];
            Keto_Sca = array[5];
            Sca_Fum = array[6];
            Fum_Mal = array[7];
            Mal_Oxo = array[8];
            OxoBalance = array[9];
            CitBalance = array[10];
        }

        public double[] ToArray()
        {
            return new double[]
            {
                Pyr_Accoa, Pyr_Oxo, AccoaOxo_Cit, Cit_Iso, Iso_Keto, Keto_Sca,
                Sca_Fum, Fum_Mal, Mal_Oxo, OxoBalance, CitBalance
            };
        }

        public KrebsRates ApplyGaussianNoise(double noise_amplitude)
        {
            Random b = new Random();
            KrebsRates r = new KrebsRates();

            double n() => 1 + noise_amplitude * b.NextGaussian();

            r.Pyr_Accoa = this.Pyr_Accoa * n();
            r.Pyr_Oxo = this.Pyr_Oxo * n();
            r.AccoaOxo_Cit = this.AccoaOxo_Cit * n();
            r.Cit_Iso = this.Cit_Iso * n();
            r.Iso_Keto = this.Iso_Keto * n();
            r.Keto_Sca = this.Keto_Sca * n();
            r.Sca_Fum = this.Sca_Fum * n();
            r.Fum_Mal = this.Fum_Mal * n();
            r.Mal_Oxo = this.Mal_Oxo * n();
            r.OxoBalance = this.OxoBalance * n();
            r.CitBalance = this.CitBalance * n();

            return r;
        }
    }
}
