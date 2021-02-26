using KrebsCycleQueuesSimulation.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KrebsCycleQueuesSimulation.Functions
{
    public static class LoadData
    {
        public static List<KrebsConstants> LoadCsv(string path)
        {
            var lines = File.ReadAllLines(path)
                .Skip(1)
                .Select(x => x.Split(';')
                    .Where(y => !string.IsNullOrEmpty(y))
                    .Select(y => double.Parse(y.Replace('.', ','))).ToArray())
                .Select(x=> new KrebsConstants(x))
                .ToList();

            return lines;
        }
    }
}
