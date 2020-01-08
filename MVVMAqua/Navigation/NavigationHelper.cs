using System.Collections.Generic;
using System.Windows;

namespace MVVMAqua.Navigation
{
	internal static class NavigationHelper
	{
		public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject? depObj) 
			where T : DependencyObject
		{
			if (depObj != null)
			{
				foreach (var rawChild in LogicalTreeHelper.GetChildren(depObj))
				{
					if (rawChild is DependencyObject child)
					{
						if (child is T x)
						{
							yield return x;
						}

						foreach (var childOfChild in FindLogicalChildren<T>(child))
						{
							yield return childOfChild;
						}
					}
				}
			}
		}
	}
}