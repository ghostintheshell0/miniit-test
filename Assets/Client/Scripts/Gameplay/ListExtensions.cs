using System.Collections.Generic;

namespace miniIT.Arcanoid
{
    public static class ListExtensions
    {
        public static T ExtractLast<T>(this List<T> list)
        {
            T item = list[^1];
            list.RemoveAt(list.Count - 1);
            return item;
        }
    }
}