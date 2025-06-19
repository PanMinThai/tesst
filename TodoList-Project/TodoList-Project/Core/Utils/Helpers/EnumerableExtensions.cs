using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.Utils.Helpers
{
    public static class EnumerableExtensions
    {
        private static readonly Random _random = new();

        public static T RandomItem<T>(this IEnumerable<T> list)
        {
            var array = list.ToArray();
            return array.Length == 0 ? default : array[_random.Next(array.Length)];
        }
    }
}
