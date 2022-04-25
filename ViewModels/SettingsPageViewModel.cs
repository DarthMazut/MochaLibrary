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
    public class SettingsPageViewModel : INavigatable
    {
        private DelegateCommand _delegateCommand;

        public SettingsPageViewModel()
        {
            Navigator = new(this, NavigationServices.MainNavigationService);
        }
        public Navigator Navigator { get; }

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
