using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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