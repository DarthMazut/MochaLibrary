using MochaCore.Navigation;
using Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class SettingsPageViewModel : INavigationParticipant
    {
        private DelegateCommand? _delegateCommand;

        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        public DelegateCommand OpenSettingsFolderCommand => _delegateCommand ??= new DelegateCommand(OpenSettingsFolder);

        private void OpenSettingsFolder()
        {
            ProcessStartInfo psi = new() 
            { 
                FileName = ApplicationSettings.SettingsFolder, 
                UseShellExecute = true 
            };

            Process.Start(psi);

        }
    }
}
