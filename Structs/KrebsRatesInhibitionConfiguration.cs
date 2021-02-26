using KrebsCycleQueuesSimulation.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace KrebsSimulationOptimized.Structs
{
    public struct KrebsRatesInhibitionConfiguration
    {
        public double Pyr_Accoa { get; set; }
        public double Pyr_Oxo { get; set; }
        public double AccoaOxo_Cit { get; }
        public double Cit_Iso { get; set; }
        public double Iso_Keto { get; set; }
        public double Keto_Sca { get; set; }
        public double Sca_Fum { get; set; }
        public double Fum_Mal { get; set; }
        public double Mal_Oxo { get; set; }
        public double Oxo_Balance { get; set; }

        public static KrebsRatesInhibitionConfiguration NoInhibition()
        {
            return new KrebsRatesInhibitionConfiguration();
        }

        public double Cit_Balance { get; set; }

    }
}
