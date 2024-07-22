using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Events;
using MochaCore.Events.Extensions;
using MochaCore.Navigation;
using MochaCore.Settings;
using ModelX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ViewModelsX.Pages
{
    public partial class SettingsPageViewModel : ObservableObject, INavigationParticipant, IOnNavigatedToAsync, IOnNavigatingFromAsync
    {
        private readonly ISettingsSectionProvider<Settings> _settingsProvider;
        private readonly IEventProvider<AppClosingEventArgs> _appClosingEventProvider;

        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        public SettingsPageViewModel()
        {
            _settingsProvider = SettingsManager.Retrieve<Settings>("Settings");
            _appClosingEventProvider = AppEventManager.RequestEventProvider<AppClosingEventArgs>("AppClosing");

            _appClosingEventProvider.Event += ApplicationClosing;
        }

        [ObservableProperty]
        private bool _isSwitched;

        [ObservableProperty]
        private SettingsOptionType _dropDownSelectedItem = SettingsOptionType.Option1Enum;

        [ObservableProperty]
        private string? _text;

        [ObservableProperty]
        private string? _password;

        [ObservableProperty]
        private string? _cryptoText;

        public async Task OnNavigatedToAsync(OnNavigatedToEventArgs e)
        {
            try
            {
                Settings settings = await _settingsProvider.LoadAsync(LoadingMode.FromOriginalSource);
                IsSwitched = settings.Switch1;
                DropDownSelectedItem = settings.OptionType;
                Text = settings.Text;
            }
            catch (IOException)
            {
                // TODO: handle
                throw;
            }
        }

        public async Task OnNavigatingFromAsync(OnNavigatingFromEventArgs e)
        {
            if (await CheckSettingsChanged())
            {
                using IDialogModule<StandardMessageDialogProperties> dialog
                    = DialogManager.RetrieveDialog<StandardMessageDialogProperties>("MessageDialog");

                dialog.Properties = new()
                {
                    Title = "Discard changes?",
                    Message = "Changes were made to the current settings, but no \"Save\" button was pressed." +
                    "\n\nDo you want to leave anyway and discard the changes?",
                    ConfirmationButtonText = "Leave & discard changes",
                    DeclineButtonText = "Stay on current page"
                };

                bool? result = await dialog.ShowModalAsync(Navigator.Module.View);

                if (result is not true)
                {
                    e.Cancel = true;
                    return;
                }
            }

            _appClosingEventProvider.Event -= ApplicationClosing;
        }

        [RelayCommand]
        private async Task Save()
        {
            try
            {
                await _settingsProvider.UpdateAsync(s =>
                {
                    s.Switch1 = IsSwitched;
                    s.OptionType = DropDownSelectedItem;
                    s.Text = Text;
                }, LoadingMode.FromOriginalSource, SavingMode.ToOriginalSource);
            }
            catch (IOException)
            {
                // TODO: handle
                throw;
            }
        }

        [RelayCommand]
        private async Task Restore()
        {
            try
            {
                await _settingsProvider.RestoreDefaultsAsync(SavingMode.ToOriginalSource);
            }
            catch (IOException)
            {
                // TODO: handle
                throw;
            }
        }

        private async Task<bool?> PromptDiscardIfRequired()
        {
            if (await CheckSettingsChanged())
            {
                using IDialogModule<StandardMessageDialogProperties> dialog
                    = DialogManager.RetrieveDialog<StandardMessageDialogProperties>("MessageDialog");

                dialog.Properties = new()
                {
                    Title = "Discard changes?",
                    Message = "Changes were made to the current settings, but no \"Save\" button was pressed." +
                    "\n\nDo you want to leave anyway and discard the changes?",
                    ConfirmationButtonText = "Leave & discard changes",
                    DeclineButtonText = "Stay on current page"
                };

                return await dialog.ShowModalAsync(Navigator.Module.View);
            }

            return;
        }

        private async Task<bool> CheckSettingsChanged()
        {
            Settings currentSettings = await _settingsProvider.LoadAsync();
            return
                currentSettings.Switch1 != IsSwitched ||
                currentSettings.OptionType != DropDownSelectedItem ||
                currentSettings.Text != Text;
        }

        private void ApplicationClosing(object? sender, AppClosingEventArgs e)
        {
            
        }
    }
}
