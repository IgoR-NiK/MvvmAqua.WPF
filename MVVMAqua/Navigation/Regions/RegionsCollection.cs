using MVVMAqua.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMAqua.Navigation.Regions
{
	public sealed class RegionsCollection
	{
		Dictionary<Type, Type> ViewModelToViewMap { get; }

		internal RegionsCollection(Dictionary<Type, Type> viewModelToViewMap)
		{
			ViewModelToViewMap = viewModelToViewMap;
		}

		public RegionNavigator GetRegionNavigator(BaseVM viewModel, string regionName)
		{
			var regionNavigator = viewModel.GetRegionNavigator(regionName);
            regionNavigator.ViewModelToViewMap = ViewModelToViewMap;
			return regionNavigator;
		}

		public RegionNavigator this[BaseVM viewModel, string regionName] => GetRegionNavigator(viewModel, regionName);
	}
}