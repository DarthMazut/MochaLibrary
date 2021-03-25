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
using MochaWPFTestApp.Views.Dialogs;
using MochaWPFTestApp.ViewModels.Dialogs;

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

            NavigationManager.AddModule(PagesIDs.Page3, () =>
            {
                return new NavigationModule(new Page3(), new Page3ViewModel());
            });

            DialogManager.DefineDialog(DialogsIDs.MsgBoxDialog, () => 
            {
                return new StandardDialogModule(this);
            });

            DialogManager.DefineDialog(DialogsIDs.OpenDialog, () =>
            {
                return new FileDialogModule(this, new Microsoft.Win32.OpenFileDialog());
            });

            DialogManager.DefineDialog(DialogsIDs.CustomDialog1, () =>
            {
                return new CustomDialogModule(this, new MyCustomDialog(), new MyCustomDialogViewModel());
            });
        }
    }
}
