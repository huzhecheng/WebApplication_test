using System;
using System.Collections.Generic;

namespace System.Linq.Extention
{
	public static class IEnumerableExtension
	{
		public static void ForEachNull<T>(this IEnumerable<T> items, Action<T> action)
		{
			if (items == null) return;
			foreach (var item in items) action(item);
		}

		public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (TSource element in source)
			{
				var elementValue = keySelector(element);
				if (seenKeys.Add(elementValue))
				{
					yield return element;
				}
			}
		}
	}
}
