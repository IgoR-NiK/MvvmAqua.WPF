using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MVVMAqua.ViewModels;

namespace MVVMAqua.Navigation.Interfaces
{
    public interface INavigator
    {        
        BaseVM ViewModel { get; }

        bool IsEmpty { get; }
        int CountViews { get; }

        void OpenFirstView();
        void OpenFirstView<T>(T viewModel) where T : BaseVM;
        void OpenFirstView<T>(T viewModel, Action<T> initialization) where T : BaseVM;
        void OpenFirstView<T>(T viewModel, Action<T> initialization, Action<T> afterViewClosed) where T : BaseVM;
        void OpenFirstView<T>(T viewModel, Action<T> initialization, Func<T, bool> afterViewClosed) where T : BaseVM;

        void NavigateTo<T>(T viewModel) where T : BaseVM;
        void NavigateTo<T>(T viewModel, Action<T> initialization) where T : BaseVM;
        void NavigateTo<T>(T viewModel, Action<T> initialization, Action<T> afterViewClosed) where T : BaseVM;
        void NavigateTo<T>(T viewModel, Action<T> initialization, Func<T, bool> afterViewClosed) where T : BaseVM;        

        void CloseLastView();
        void CloseLastView(bool isCallbackCloseViewHandler);

        void CloseAllViews();
        void CloseAllViews(bool isCallbackCloseViewHandler);
    }
}