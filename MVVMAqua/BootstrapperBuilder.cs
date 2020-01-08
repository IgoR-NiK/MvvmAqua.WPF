using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using MVVMAqua.Helpers;
using MVVMAqua.ViewModels;
using MVVMAqua.Views;

namespace MVVMAqua
{
	public class BootstrapperBuilder
	{
		private Bootstrapper _bootstrapper = new Bootstrapper();
		private List<Assembly> _assemblies = new List<Assembly>();
		
		private readonly Assembly _callingAssembly;

		private bool _isAutoMappingViewModelToView;
		private bool _isFirstMapping;

		private IEnumerable<Type>? _viewModels;


		public BootstrapperBuilder()
		{
			_callingAssembly = Assembly.GetCallingAssembly();

			Reset();
		}


		public BootstrapperBuilder SearchInNextAssemblies(params Assembly[] assemblies)
		{
			_assemblies = new List<Assembly>(assemblies);

			return this;
		}

		public BootstrapperBuilder SetAutoMappingViewModelToView(bool isAutoMappingViewModelToView)
		{
			_isAutoMappingViewModelToView = isAutoMappingViewModelToView;

			return this;
		}

		#region Настройка модального и обычного окон

		public BootstrapperBuilder SetModalWindowColorTheme(Color modalWindowColorTheme)
		{
			_bootstrapper.ModalWindowColorTheme = modalWindowColorTheme;

			return this;
		}

		public BootstrapperBuilder SetWindowType<T>() 
			where T : Window, new()
		{
			_bootstrapper.SetWindowType<T>();

			return this;
		}

		#endregion

		#region Привязка View к ViewModel

		public BootstrapperBuilder WhereVM(Func<Type, bool>? predicate)
		{
			_viewModels = _assemblies.SelectMany(assembly => assembly
				.GetTypes()
				.Where(x => 
					typeof(BaseVM).IsAssignableFrom(x) && 
					(predicate?.Invoke(x) ?? true)));

			return this;
		}

		public BootstrapperBuilder WhereView(Func<Type, bool>? predicate)
		{
			if (_viewModels != null)
			{
				var views = _assemblies.SelectMany(assembly => assembly
					.GetTypes()
					.Where(x =>
						typeof(ContentControl).IsAssignableFrom(x) &&
						x.GetConstructor(Type.EmptyTypes) != null &&
						(predicate?.Invoke(x) ?? true)))
					.ToArray();

				MappingViewModelBaseView(views);

				if (_isAutoMappingViewModelToView)
				{
					AutoMappingViewModelToView(_viewModels, views);
				}

				_isFirstMapping = false;
			}

			return this;
		}

		/// <summary>
		/// Привязка VM к View, унаследованных от BaseView{T} where T : BaseVM.
		/// Если представление создано с помощью BaseView привязывать вручную его не обязательно.
		/// Представление будет привязано к ViewModel типа T.
		/// </summary>
		private void MappingViewModelBaseView(IEnumerable<Type> views)
		{
			views.ForEach(view =>
			{
				if (typeof(IBaseView<BaseVM>).IsAssignableFrom(view))
				{
					var vm = view
						.GetProperty(nameof(IBaseView<BaseVM>.ViewModel))
						?.PropertyType;

					if (vm != null && !_bootstrapper.ViewModelToViewMap.ContainsKey(vm))
					{
						_bootstrapper.ViewModelToViewMap.Add(vm, view);
					}
				}
			});
		}

		/// <summary>
		/// Автоматическая привязка VM к View. 
		/// VM должна иметь следующие названия: Name, NameVM или NameViewModel. 
		/// View должна иметь следующие названия: Name или NameView. 
		/// Регистр значения не имеет.
		/// </summary>
		private void AutoMappingViewModelToView(IEnumerable<Type> viewModels, IEnumerable<Type> views)
		{
			viewModels.ForEach(vm =>
			{
				if (!_bootstrapper.ViewModelToViewMap.ContainsKey(vm))
				{
					var viewModelName = vm.Name.ToUpper();

					viewModelName = Regex.Replace(viewModelName, "(VM|VIEWMODEL)$", "");

					var view = views
						.SingleOrDefault(
							v => v.Name.ToUpper() == viewModelName 
							     || v.Name.ToUpper() == $"{viewModelName}VIEW");

					if (view != null)
					{
						_bootstrapper.ViewModelToViewMap.Add(vm, view);
					}
				}
			});
		}

		
		private Type? _tempViewModel;

		public BootstrapperBuilder Bind<T>() 
			where T : BaseVM
		{
			if (_bootstrapper.ViewModelToViewMap.ContainsKey(typeof(T)))
			{
				throw new ArgumentException($"The type {typeof(T).Name} is already registered");
			}

			_tempViewModel = typeof(T);
			return this;
		}

		public BootstrapperBuilder To<T>() 
			where T : ContentControl, new()
		{
			if (_tempViewModel != null)
			{
				_bootstrapper.ViewModelToViewMap.Add(_tempViewModel, typeof(T));
				_tempViewModel = null;
			}

			return this;
		}

		#endregion


		public void Reset()
		{
			_bootstrapper = new Bootstrapper();
			_assemblies = new List<Assembly> { _callingAssembly };
			_isAutoMappingViewModelToView = true;
			_isFirstMapping = true;
			_viewModels = null;
		}

		public Bootstrapper Build()
		{
			if (_isFirstMapping)
			{
				WhereVM(null);
				WhereView(null);
			}

			return _bootstrapper;
		}
	}
}