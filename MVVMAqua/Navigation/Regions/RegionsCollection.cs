﻿using MvvmAqua.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmAqua.Navigation.Regions
{
	public class RegionsCollection
	{
		Dictionary<Type, Type> ViewModelToViewMap { get; }

		public RegionsCollection(Dictionary<Type, Type> viewModelToViewMap)
		{
			ViewModelToViewMap = viewModelToViewMap;
		}

		public RegionWrapper GetRegion(BaseVM viewModel, string regionName)
		{
			var regionWrapper = viewModel.GetRegionWrapper(regionName);
			regionWrapper.ViewModelToViewMap = ViewModelToViewMap;
			return regionWrapper;
		}

		public RegionWrapper this[BaseVM viewModel, string regionName] => GetRegion(viewModel, regionName);
	}
}
