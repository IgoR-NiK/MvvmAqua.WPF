using MVVMAqua.Navigation.Interfaces;
using MVVMAqua.ViewModels;

namespace MVVMAqua.Navigation.Regions
{
	public sealed class RegionsCollection
	{
        public INavigator GetRegionNavigator(BaseVM viewModel, string regionName) => viewModel.RegionNavigators[regionName];
        public INavigator this[BaseVM viewModel, string regionName] => viewModel.RegionNavigators[regionName];
	}
}