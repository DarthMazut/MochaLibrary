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
    public partial class SettingsPageViewModel : ObservableObject, INavigationParticipant
    {
        public INavigator Navigator => MochaCore.Navigation.Navigator.Create();

        public string Title => "Hello there: Settings Page";

        [RelayCommand]
        private async Task Test()
        {
            ISettingsSectionProvider<Settings> settingsSection = SettingsManager.Retrieve<Settings>("Settings");
            Settings settings = await settingsSection.LoadAsync();

            await settingsSection.UpdateAsync(s =>
            {
                s.MyTextSetting = "dupa";
            });
        }
    }
}
