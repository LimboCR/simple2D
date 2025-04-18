using System;
using System.Collections.Generic;
using UnityEngine;

namespace Limbo.CollectionUtils
{
    public static class DictionaryUtils
    {
        /// <summary>
        /// Populates a new dictionary from a list using selectors.
        /// Logs a warning on duplicate keys.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="list">List to populate from</param>
        /// <param name="keySelector">Values to use as Dictionary keys</param>
        /// <param name="valueSelector">Values to use as Dictionary values</param>
        /// <param name="logDuplicates">Whether to log duplicates or not (default true)</param>
        /// <returns>Populated dictionary</returns>
        public static Dictionary<TKey, TValue> PopulateDictionary<TItem, TKey, TValue>(
            List<TItem> list,
            Func<TItem, TKey> keySelector,
            Func<TItem, TValue> valueSelector,
            bool logDuplicates = true)
        {
            var dict = new Dictionary<TKey, TValue>();
            PopulateIntoExistingDictionary(dict, list, keySelector, valueSelector, logDuplicates);
            return dict;
        }

        /// <summary>
        /// Populates a new dictionary from a list using selectors and optional filter.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="list">List to populate from</param>
        /// <param name="keySelector">Values to use as Dictionary keys</param>
        /// <param name="valueSelector">Values to use as Dictionary values</param>
        /// <param name="filter">Filtering condition</param>
        /// <param name="logDuplicates">Whether to log duplicates or not (default true)</param>
        /// <returns>Populated dictionary</returns>
        public static Dictionary<TKey, TValue> PopulateDictionary<TItem, TKey, TValue>(
            List<TItem> list,
            Func<TItem, TKey> keySelector,
            Func<TItem, TValue> valueSelector,
            Func<TItem, bool> filter = null,
            bool logDuplicates = true)
        {
            var dict = new Dictionary<TKey, TValue>();
            PopulateIntoExistingDictionary(dict, list, keySelector, valueSelector, filter, logDuplicates);
            return dict;
        }

        /// <summary>
        /// Populates an existing dictionary from a list using selectors.
        /// Skips duplicates and optionally logs them.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="targetDict">Dictionary to populate</param>
        /// <param name="list">List to populate from</param>
        /// <param name="keySelector">Values to use as Dictionary keys</param>
        /// <param name="valueSelector">Values to use as Dictionary values</param>
        /// <param name="logDuplicates">Whether to log duplicates or not (default true)</param>
        public static void PopulateIntoExistingDictionary<TItem, TKey, TValue>(
            Dictionary<TKey, TValue> targetDict,
            List<TItem> list,
            Func<TItem, TKey> keySelector,
            Func<TItem, TValue> valueSelector,
            bool logDuplicates = true)
        {
            if (list == null || targetDict == null)
                return;

            foreach (var item in list)
            {
                if (item == null) continue;

                TKey key = keySelector(item);
                TValue value = valueSelector(item);

                if (!targetDict.ContainsKey(key))
                {
                    targetDict.Add(key, value);
                }
                else if (logDuplicates)
                {
                    Debug.LogWarning($"Duplicate key '{key}' found. Skipping entry.");
                }
            }
        }

        /// <summary>
        /// Populates an existing dictionary from a list using selectors and optional filter.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="targetDict">Dictionary to populate</param>
        /// <param name="list">List to populate from</param>
        /// <param name="keySelector">Values to use as Dictionary keys</param>
        /// <param name="valueSelector">Values to use as Dictionary values</param>
        /// <param name="filter">Filtering condition</param>
        /// <param name="logDuplicates">Whether to log duplicates or not (default true)</param>
        public static void PopulateIntoExistingDictionary<TItem, TKey, TValue>(
            Dictionary<TKey, TValue> targetDict,
            List<TItem> list,
            Func<TItem, TKey> keySelector,
            Func<TItem, TValue> valueSelector,
            Func<TItem, bool> filter = null,
            bool logDuplicates = true)
        {
            if (list == null || targetDict == null)
                return;

            foreach (var item in list)
            {
                if (item == null) continue;
                if (filter != null && !filter(item)) continue;

                TKey key = keySelector(item);
                TValue value = valueSelector(item);

                if (!targetDict.ContainsKey(key))
                {
                    targetDict.Add(key, value);
                }
                else if (logDuplicates)
                {
                    Debug.LogWarning($"Duplicate key '{key}' found. Skipping entry.");
                }
            }
        }
    }

    public static class CollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="valueSelector"></param>
        /// <param name="filter"></param>
        /// <param name="logDuplicates"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDictionarySafe<TItem, TKey, TValue>(
            this IEnumerable<TItem> source,
            Func<TItem, TKey> keySelector,
            Func<TItem, TValue> valueSelector,
            Func<TItem, bool> filter = null,
            bool logDuplicates = true)
        {
            var dict = new Dictionary<TKey, TValue>();
            dict.PopulateFrom(source, keySelector, valueSelector, filter, logDuplicates);
            return dict;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="valueSelector"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDictionarySafe<TItem, TKey, TValue>(
            this IEnumerable<TItem> source,
            Func<TItem, TKey> keySelector,
            Func<TItem, TValue> valueSelector)
        {
            return source.ToDictionarySafe(keySelector, valueSelector, null, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="valueSelector"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDictionarySafeNoLog<TItem, TKey, TValue>(
            this IEnumerable<TItem> source,
            Func<TItem, TKey> keySelector,
            Func<TItem, TValue> valueSelector)
        {
            return source.ToDictionarySafe(keySelector, valueSelector, null, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="valueSelector"></param>
        public static void PopulateFromNoLog<TItem, TKey, TValue>(
            this Dictionary<TKey, TValue> target,
            IEnumerable<TItem> source,
            Func<TItem, TKey> keySelector,
            Func<TItem, TValue> valueSelector)
        {
            target.PopulateFrom(source, keySelector, valueSelector, null, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="valueSelector"></param>
        public static void PopulateFrom<TItem, TKey, TValue>(
            this Dictionary<TKey, TValue> target,
            IEnumerable<TItem> source,
            Func<TItem, TKey> keySelector,
            Func<TItem, TValue> valueSelector)
        {
            target.PopulateFrom(source, keySelector, valueSelector, null, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="valueSelector"></param>
        /// <param name="filter"></param>
        /// <param name="logDuplicates"></param>
        public static void PopulateFrom<TItem, TKey, TValue>(
            this Dictionary<TKey, TValue> target,
            IEnumerable<TItem> source,
            Func<TItem, TKey> keySelector,
            Func<TItem, TValue> valueSelector,
            Func<TItem, bool> filter = null,
            bool logDuplicates = true)
        {
            if (source == null || target == null) return;

            foreach (var item in source)
            {
                if (item == null) continue;
                if (filter != null && !filter(item)) continue;

                var key = keySelector(item);
                var value = valueSelector(item);

                if (!target.ContainsKey(key))
                {
                    target.Add(key, value);
                }
                else if (logDuplicates)
                {
                    Debug.LogWarning($"Duplicate key '{key}' found. Skipping.");
                }
            }
        }

        // Overload with instantiation and optional setup
        public static Dictionary<TKey, TValue> ToDictionaryInstanced<TItem, TKey, TValue>(
            this IEnumerable<TItem> source,
            Func<TItem, TValue> instancer,                   // Instantiate/clone function
            Func<TValue, TKey> keySelector,                  // After instancing, select key
            Action<TValue> initializer = null,               // Optional initializer
            Func<TItem, bool> filter = null,                 // Optional filter
            bool logDuplicates = true)
            where TValue : UnityEngine.Object
        {
            var dict = new Dictionary<TKey, TValue>();

            foreach (var item in source)
            {
                if (item == null) continue;
                if (filter != null && !filter(item)) continue;

                TValue instance = instancer(item); // Usually Instantiate(item)
                initializer?.Invoke(instance);     // Call your Initialize()

                var key = keySelector(instance);

                if (!dict.ContainsKey(key))
                    dict.Add(key, instance);
                else if (logDuplicates)
                    Debug.LogWarning($"Duplicate key '{key}' found. Skipping.");
            }

            return dict;
        }
    }
}