using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Dispatching;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelsX.Dialogs;

namespace ViewModelsX.Windows
{
    public partial class WindowingGeneralWindowViewModel : ObservableObject, IWindowAware<WindowingGeneralWindowProperties>
    {
        private readonly Lazy<DialogQueue> _dialogQueue;

        private DialogQueue DialogQueue => _dialogQueue.Value;

        public IWindowControl<WindowingGeneralWindowProperties> WindowControl { get; }
            = new WindowControl<WindowingGeneralWindowProperties>();

        public WindowingGeneralWindowViewModel()
        {
            _dialogQueue = new(() => new DialogQueue(WindowControl.View));

            //Replace `Opened` with `Customize` when #33 is resolved.
            //WindowControl.Customize(p => Text = $"Hello, parameter: {p.Parameter}");
            WindowControl.Opened += WindowOpened;
            WindowControl.StateChanged += WindowStateChanged;
            WindowControl.Closing += WindowClosing;
            WindowControl.Closed += WindowClosed;
        }

        [ObservableProperty]
        private string? _inputParameterText;

        [ObservableProperty]
        private string? _outputParameterText;

        [RelayCommand]
        private void Maximize() => WindowControl.Maximize();

        [RelayCommand]
        private void Minimize() => WindowControl.Minimize();

        [RelayCommand]
        private void Hide() => WindowControl.Hide();

        [RelayCommand]
        private void Close() => WindowControl.Close();

        private void WindowOpened(object? sender, EventArgs e)
        {
            UpdateParameterText();
            using ICustomDialogModule<StandardMessageDialogProperties> dialogModule = AppDialogs.StandardMessageDialog.Module;
            dialogModule.Properties.Title = "Event fired ⚡";
            dialogModule.Properties.Message = nameof(WindowControl.Opened);
            dialogModule.Properties.ConfirmationButtonText = $"Cool 🙄";

            DialogQueue.EnqueueDialog(dialogModule);
        }

        private void WindowStateChanged(object? sender, WindowStateChangedEventArgs e)
        {
            if (e.WindowState == ModuleWindowState.Closed)
            {
                return;
            }

            using ICustomDialogModule<StandardMessageDialogProperties> dialogModule = AppDialogs.StandardMessageDialog.Module;
            dialogModule.Properties = new StandardMessageDialogProperties()
            {
                Title = "Event fired ⚡",
                Message = $"{nameof(WindowControl.StateChanged)}: new state is: {e.WindowState}",
                ConfirmationButtonText = $"Cool 🙄"
            };

            DialogQueue.EnqueueDialog(dialogModule);
        }

        private void WindowClosing(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;

            using ICustomDialogModule<StandardMessageDialogProperties> dialogModule = AppDialogs.StandardMessageDialog.Module;
            dialogModule.Properties = new StandardMessageDialogProperties()
            {
                Title = "Closing",
                Message = "Are you sure you want to close the window?",
                ConfirmationButtonText = "Yeah!",
                CancelButtonText = "Nope."
            };

            DialogQueue.EnqueueDialog(dialogModule, result =>
            {
                if (result is true)
                {
                    WindowControl.Close();
                }
            });
        }

        private void WindowClosed(object? sender, EventArgs e)
        {
            WindowControl.Properties.OutputParameter = OutputParameterText;
            DialogQueue.Dispose();
        }

        private void UpdateParameterText()
        {
            if (WindowControl.Properties.InputParameter is string param)
            {
                InputParameterText = $"Provided parameter: {param}";
            }
            else
            {
                InputParameterText = "No parameter provided";
            }
        }
    }
}
