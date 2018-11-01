using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMAqua.Navigation
{
	static class NavigationHelper
	{
		public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj) where T : DependencyObject
		{
			if (depObj != null)
			{
				foreach (object rawChild in LogicalTreeHelper.GetChildren(depObj))
				{
					if (rawChild is DependencyObject child)
					{
						if (child is T x)
						{
							yield return x;
						}

						foreach (T childOfChild in FindLogicalChildren<T>(child))
						{
							yield return childOfChild;
						}
					}
				}
			}
		}
	}
}