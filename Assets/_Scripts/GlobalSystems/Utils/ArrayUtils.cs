using System.Collections.Generic;
using UnityEngine;

namespace Limbo.ArrayUtils
{
    public static class ArrayUtils
    {
        public static void ListCleaner<T>(List<T> list)
        {
            list.RemoveAll(item => item == null);
        }
    }

    public static class ListExtensions
    {
        public static void Cleaner<T>(this List<T> list)
        {
            list.RemoveAll(item => item == null);
        }

        public static void ComponentCleaner<T>(List<T> list) where T : Component
        {
            list.RemoveAll(item => item == null);
        }

        public static void ReplaceNullsWith<T>(this List<T> list, T fallbackValue)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                    list[i] = fallbackValue;
            }
        }
    }
}