using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMAqua.Helpers
{
	static class IEnumerableHelper
	{
		public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
		{
			if (list == null) throw new ArgumentNullException("list");
			if (action == null) throw new ArgumentNullException("action");

			foreach (var elem in list)
				action(elem);
		}
	}
}