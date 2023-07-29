using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Dispatching;
using MochaCore.NavigationEx;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class BlankPage1ViewModel : BindableBase, INavigationParticipant
    {
        private bool _isActive;
        private CancellationTokenSource _cts = new();

        public BlankPage1ViewModel()
        {
            NavigateCommand = new DelegateCommand(Navigate);
            OpenDialogCommand = new DelegateCommand(OpenDialog);
        }

        public INavigator Navigator { get; } = MochaCore.NavigationEx.Navigator.Create();

        public bool IsActive 
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public DelegateCommand NavigateCommand { get; }

        public DelegateCommand OpenDialogCommand { get; }

        

        private async void Navigate()
        {
            await Navigator.NavigateAsync(Pages.PeoplePage.Id);
        }

        private async void OpenDialog()
        {
            IDialogModule<StandardMessageDialogProperties> dialog = Dialogs.MoreInfoDialog.Module;
            dialog.Properties.Title = "Hello";
            dialog.Properties.Message = "Hello There!";
            dialog.Properties.ConfirmationButtonText = "OK";
            dialog.Properties.DeclineButtonText= "Not OK";
            dialog.Properties.Icon = StandardMessageDialogIcons.Error;
            if (dialog is ICustomDialogModule<StandardMessageDialogProperties> customDialog)
            {
                _ = Task.Delay(3000).ContinueWith(t => DispatcherManager.GetMainThreadDispatcher().EnqueueOnMainThread(() => customDialog.Close(false)));
            }
            bool? result = await dialog.ShowModalAsync(this);
            Debug.WriteLine(result);
        }
    }
}
