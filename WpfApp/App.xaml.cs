using System.Windows;
using MVVMAqua;
using WpfApp.ViewModels;

namespace WpfApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			var bootstrapper = new BootstrapperBuilder().Build();
			bootstrapper.OpenNewWindow(new MainVM());
		}
	}
}
