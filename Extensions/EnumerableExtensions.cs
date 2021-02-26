using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KrebsCycleQueuesSimulation.Extensions
{
    public static class EnumerableExtensions
    {
        public static T[,] To2DArray<T>(this IEnumerable<IEnumerable<T>> source)
        {
            var data = source
                .Select(x => x.ToArray())
                .ToArray();

            var res = new T[data.Length, data.Max(x => x.Length)];
            for (var i = 0; i < data.Length; ++i)
            {
                for (var j = 0; j < data[i].Length; ++j)
                {
                    res[i, j] = data[i][j];
                }
            }

            return res;
        }

        public static T[,] Transpose<T>(this T[,] array)
        {
            var to_return = new T[array.GetLength(1), array.GetLength(0)];

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    to_return[j, i] = array[i, j];
                }
            }

            return to_return;
        }

        public static IEnumerable<T[]> ToList<T>(this T[,] array)
        {
            int height = array.GetLength(0);
            int width = array.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                T[] arr = new T[width];
                array.CopyTo(arr, i * height);
                yield return arr;
            }
        }

        public static IEnumerable<T> AllExcept<T>(this IEnumerable<T> list, int index)
        {
            var before = list.Take(index);
            var after = list.Skip(index + 1);
            return before.Concat(after);
        }
    }
}
