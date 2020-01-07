using System;
using System.Collections.Generic;

namespace MVVMAqua.Helpers
{
	public static class EnumerableHelper
	{
		public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
		{
			if (list == null) throw new ArgumentNullException(nameof(list));
			if (action == null) throw new ArgumentNullException(nameof(action));

			foreach (var elem in list)
				action(elem);
		}
	}
}