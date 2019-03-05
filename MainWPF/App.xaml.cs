using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using MVVMAqua;
using MainWPF.ViewModels;

namespace MainWPF
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			var bootstrapper = new Bootstrapper();
			bootstrapper.OpenNewWindow(new MainVM(), vm => vm.Title.Value = "1", null, null, vm => vm.ViewNavigator.ShowDialog(new MainVM(), x => x.Title.Value = "title", "", MVVMAqua.Enums.ModalButtons.OkCancel));		
		}
	}
}