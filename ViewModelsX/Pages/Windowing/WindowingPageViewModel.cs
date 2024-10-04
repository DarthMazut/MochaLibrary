using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Navigation;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelsX.Dialogs;
using ViewModelsX.Windows;

namespace ViewModelsX.Pages.Windowing
{
    public partial class WindowingPageViewModel : ObservableObject, INavigationParticipant, IOnNavigatedTo, IOnNavigatedFrom
    {
        private static readonly string _noDataString = "NULL";

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

        public string IsOpenProperty => _windowModule?.IsOpen.ToString() ?? _noDataString;

        public string WindowStateProperty => _windowModule?.WindowState.ToString() ?? _noDataString;

        public string IsDisposedProperty => _windowModule?.IsDisposed.ToString() ?? _noDataString;

        public bool CanCreateWindow => _windowModule is null;

        public bool CanOpenWindow => _windowModule is not null && !_windowModule.IsOpen;

        public bool CanMaximizeWindow => _windowModule is not null && _windowModule.IsOpen && _windowModule.WindowState != ModuleWindowState.Maximized;

        public bool CanMinimizeWindow => _windowModule is not null && _windowModule.IsOpen && _windowModule.WindowState != ModuleWindowState.Minimized;

        public bool CanHideWindow => _windowModule is not null && _windowModule.IsOpen && _windowModule.WindowState != ModuleWindowState.Hidden;

        public bool CanRestoreWindow => _windowModule is not null && _windowModule.IsOpen && _windowModule.WindowState != ModuleWindowState.Normal;

        public bool CanCloseWindow => _windowModule is not null && _windowModule.IsOpen;

        public bool CanDisposeWindow => _windowModule is not null;


        [RelayCommand]
        private void CreateWindow() => SafeExecute(() =>
        {
            _windowModule = AppWindows.WindowingGeneralWindow.Module;
            SubscribeAll();
            AddLogMessage("Created window module");  
        });

        [RelayCommand]
        private Task OpenWindow() => SafeExecute(async () =>
        {
            ICustomDialogModule<Dialogs.WindowingPageOpenWindowDialogProperties> propertiesDialogModule
                = AppDialogs.OpenWindowPropertiesDialog.Module;

            if (await propertiesDialogModule.ShowModalAsync(Navigator.Module.View) is true)
            {
                _windowModule!.Properties.InputParameter = propertiesDialogModule.Properties.Parameter;
                await _windowModule!.OpenAsync();
                AddLogMessage($"Output parameter: {_windowModule.Properties.OutputParameter ?? _noDataString}");
            }   
        });

        [RelayCommand]
        private void MaximizeWindow() => SafeExecute(() => _windowModule!.Maximize());

        [RelayCommand]
        private void MinimizeWindow() => SafeExecute(() => _windowModule!.Minimize());

        [RelayCommand]
        private void HideWindow() => SafeExecute(() => _windowModule!.Hide());

        [RelayCommand]
        private void RestoreWindow() => SafeExecute(() => _windowModule!.Restore());

        [RelayCommand]
        private void CloseWindow() => SafeExecute(() => _windowModule!.Close());

        [RelayCommand]
        private void DisposeWindow() => SafeExecute(() =>
        {
            _windowModule!.Dispose();
            UnsubscribeAll();
            _windowModule = null;
        });

        private void UpdateState()
        {
            OnPropertyChanged(nameof(CanCreateWindow));
            OnPropertyChanged(nameof(CanOpenWindow));
            OnPropertyChanged(nameof(CanMaximizeWindow));
            OnPropertyChanged(nameof(CanMinimizeWindow));
            OnPropertyChanged(nameof(CanHideWindow));
            OnPropertyChanged(nameof(CanRestoreWindow));
            OnPropertyChanged(nameof(CanCloseWindow));
            OnPropertyChanged(nameof(CanDisposeWindow));
            OnPropertyChanged(nameof(IsOpenProperty));
            OnPropertyChanged(nameof(WindowStateProperty));
            OnPropertyChanged(nameof(IsDisposedProperty));
        }

        private void SubscribeAll()
        {
            if (_windowModule is not null)
            {
                _windowModule.Opened += ModuleOpenedHandler;
                _windowModule.StateChanged += ModuleStateChangedHandler;
                _windowModule.Closing += ModuleClosingHandler;
                _windowModule.Closed += ModuleClosedHandler;
                _windowModule.Disposed += ModuleDisposedHandler;
            }
        }

        private void UnsubscribeAll()
        {
            if (_windowModule is not null)
            {
                _windowModule.Opened -= ModuleOpenedHandler;
                _windowModule.StateChanged -= ModuleStateChangedHandler;
                _windowModule.Closing -= ModuleClosingHandler;
                _windowModule.Closed -= ModuleClosedHandler;
                _windowModule.Disposed -= ModuleDisposedHandler;
            }
        }

        private void ModuleOpenedHandler(object? sender, EventArgs e)
        {
            AddLogMessage("Module opened");
            UpdateState();
        }

        private void ModuleStateChangedHandler(object? sender, WindowStateChangedEventArgs e)
        {
            AddLogMessage($"State changed to {e.WindowState}");
            UpdateState();
        }

        private void ModuleClosingHandler(object? sender, CancelEventArgs e)
        {
            AddLogMessage($"Module closing...");
            UpdateState();
        }

        private void ModuleClosedHandler(object? sender, EventArgs e)
        {
            AddLogMessage("Module closed");
            UpdateState();
        }

        private void ModuleDisposedHandler(object? sender, EventArgs e)
        {
            AddLogMessage("Module disposed");
            UpdateState();
        }

        private void AddLogMessage(string message)
            => LogText += message + Environment.NewLine;

        private Task SafeExecute(Action action) => SafeExecute(action, null);

        private Task SafeExecute(Func<Task> asyncAction) => SafeExecute(null, asyncAction);

        private async Task SafeExecute(Action? action, Func<Task>? asyncAction)
        {
            try
            {
                action?.Invoke();
                if (asyncAction is not null)
                {
                    Task actionTask = asyncAction.Invoke();
                    UpdateState();
                    await actionTask;
                }

                UpdateState();
            }
            catch (Exception ex) // do not catch general exceptions :(
            {
                ICustomDialogModule<StandardMessageDialogProperties> dialogModule = AppDialogs.StandardMessageDialog.Module;
                dialogModule.Properties = new()
                {
                    Title = "Something went wrong 💀",
                    Message = ex.ToString()
                };

                await dialogModule.ShowModalAsync(Navigator.Module.View);
            }
        }
    }
}
