using System;
using System.ComponentModel;
using System.Windows;

using MVVMAqua.Navigation.Interfaces;

namespace MVVMAqua.Windows
{
	public abstract class BaseWindow : Window
	{
        internal bool IsCallbackCloseWindowHandler { get; set; } = true;
        internal Func<bool>? WindowClosing { get; set; }
		
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
            
            if (IsCallbackCloseWindowHandler)
            {
                e.Cancel = DataContext is IWindowClosing windowClosing ? !windowClosing.WindowClosingAction() : !WindowClosing?.Invoke() ?? false;
            }
		}
	}
}