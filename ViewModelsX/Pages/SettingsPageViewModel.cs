using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Navigation;
using MochaCore.Settings;
using ModelX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Pages
{
    public partial class SettingsPageViewModel : ObservableObject, INavigationParticipant, IOnNavigatedToAsync
    {
        private readonly ISettingsSectionProvider<Settings> _settingsProvider;

        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        public SettingsPageViewModel()
        {
            _settingsProvider = SettingsManager.Retrieve<Settings>("Settings");
        }

        [ObservableProperty]
        private bool _isSwitched;

        [ObservableProperty]
        private SettingsOptionType _dropDownSelectedItem = SettingsOptionType.Option1Enum;

        [ObservableProperty]
        private string? _text;

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


    }
}
