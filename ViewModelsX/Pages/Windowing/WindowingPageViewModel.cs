using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Navigation;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelsX.Windows;

namespace ViewModelsX.Pages.Windowing
{
    public partial class WindowingPageViewModel : ObservableObject, INavigationParticipant, IOnNavigatedTo, IOnNavigatedFrom
    {
        private IWindowModule<WindowingGeneralWindowProperties>? _windowModule;

        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        public void OnNavigatedTo(OnNavigatedToEventArgs e)
        {
            _windowModule = WindowManager.GetCreatedModules(AppWindows.WindowingGeneralWindow.Id)
                .FirstOrDefault() as IWindowModule<WindowingGeneralWindowProperties>;

            SubscribeAll();
            UpdateState();
        }

        public void OnNavigatedFrom(OnNavigatedFromEventArgs e)
        {
            UnsubscribeAll();
        }

        [ObservableProperty]
        private string _logText = string.Empty;

        public bool CanCreateWindow => _windowModule is null;

        public bool CanOpenWindow => _windowModule is not null && !_windowModule.IsOpen;

        public bool CanMaximizeWindow => false;

        public bool CanMinimizeWindow => false;

        public bool CanHideWindow => false;

        public bool CanRestoreWindow => false;

        public bool CanCloseWindow => false;

        public bool CanDisposeWindow => _windowModule is not null;


        [RelayCommand]
        private void CreateWindow()
        {
            _windowModule = AppWindows.WindowingGeneralWindow.Module;
            SubscribeAll();
            AddLogMessage("Created window module");
            UpdateState();
        }

        [RelayCommand]
        private async Task OpenWindow()
        {
            await _windowModule!.OpenAsync();
            UpdateState();
        }

        [RelayCommand]
        private void DisposeWindow()
        {
            _windowModule!.Dispose();
            _windowModule = null;
            UpdateState();
        }

        private void UpdateState()
        {
            OnPropertyChanged(nameof(CanCreateWindow));
            OnPropertyChanged(nameof(CanOpenWindow));
            OnPropertyChanged(nameof(CanDisposeWindow));
        }

        private void SubscribeAll()
        {
            if (_windowModule is not null)
            {
                _windowModule.Opened += ModuleOpenedHandler;
                _windowModule.StateChanged += ModuleStateChangedHandler;
            }
        }

        private void UnsubscribeAll()
        {
            if (_windowModule is not null)
            {
                _windowModule.Opened -= ModuleOpenedHandler;
                _windowModule.StateChanged -= ModuleStateChangedHandler;
            }
        }

        private void ModuleOpenedHandler(object? sender, EventArgs e)
            => AddLogMessage("Module opened");

        private void ModuleStateChangedHandler(object? sender, WindowStateChangedEventArgs e)
            => AddLogMessage($"State changed to {e.WindowState}");

        private void AddLogMessage(string message)
            => LogText += message + Environment.NewLine;
    }
}
