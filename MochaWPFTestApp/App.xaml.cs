using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Mocha.Navigation;
using Mocha.Dialogs;
using MochaWPFTestApp.Views;
using MochaWPFTestApp.ViewModels;
using MochaWPF;

namespace MochaWPFTestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            NavigationManager.AddModule(PagesIDs.Page1, () =>
            {
                return new NavigationModule(
                    new Page1(),
                    new Page1ViewModel());
            });

            NavigationManager.AddModule(PagesIDs.Page2, () =>
            {
                return new NavigationModule(
                    new Page2(),
                    new Page2ViewModel());
            });

            DialogManager.DefineDialog(DialogsIDs.OpenDialog, () =>
            {
                return new WindowsDialogModule(
                    new Microsoft.Win32.SaveFileDialog(),
                    null,
                    (w) => 
                    {
                        Window window = null;
                        Current.Dispatcher.Invoke(() =>
                        {
                            window = Current.Windows[0];
                        });

                        return window;
                    });
            });
        }
    }
}
