using MVVMAqua.Navigation.Interfaces;
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
        Bootstrapper Bootstrapper { get; }

        internal RegionsCollection(Bootstrapper bootstrapper)
        {
            Bootstrapper = bootstrapper;
        }

        public INavigator GetRegionNavigator(BaseVM viewModel, string regionName) => viewModel.GetRegionNavigator(regionName, Bootstrapper);

		public INavigator this[BaseVM viewModel, string regionName] => viewModel.GetRegionNavigator(regionName, Bootstrapper);
	}
}