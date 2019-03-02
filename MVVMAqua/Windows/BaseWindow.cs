using System;
using System.ComponentModel;
using System.Windows;

using MVVMAqua.Navigation.Interfaces;

namespace MVVMAqua.Windows
{
	public abstract class BaseWindow : Window
	{
        internal bool IsCallbackCloseWindowHandler { get; set; }
        internal Func<bool> WindowClosing { get; set; }
		
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
            
            if (IsCallbackCloseWindowHandler)
            {
                e.Cancel = DataContext is IWindowCloser windowCloser ? !windowCloser.CloseWindow() : !WindowClosing?.Invoke() ?? false;
            }
		}
	}
}