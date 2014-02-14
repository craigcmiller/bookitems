using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookItems.Core
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds the contents of <paramref name="itemsToAdd"/> to <paramref name="list"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="itemsToAdd"></param>
        public static void AddCollection<T>(this IList<T> list, IEnumerable<T> itemsToAdd)
        {
            foreach (T item in itemsToAdd)
                list.Add(item);
        }
    }
}
