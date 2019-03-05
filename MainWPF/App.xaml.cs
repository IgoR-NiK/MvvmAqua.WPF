using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using MVVMAqua;
using MainWPF.ViewModels;
using MainWPF.Views;

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

            bootstrapper.Bind<MyVM<MainVM>>().To<MyView>();

            bootstrapper.OpenNewWindow(new MainVM(), vm => vm.Title.Value = "1", null, null,
                y => y.ViewNavigator.ShowDialog(
                    new MyModalWindow(),
                    new MyVM<MainVM>(new MainVM(), x => x.Title.Value = "title", "Уведомление", MVVMAqua.Enums.ModalButtons.OkCancel, "Ок", "Отмена", System.Windows.Media.Color.FromRgb(0x4A, 0x76, 0xC9))));		
		}
	}
}