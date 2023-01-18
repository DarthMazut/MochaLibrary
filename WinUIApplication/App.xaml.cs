using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using MochaCore.Behaviours;
using MochaCore.DialogsEx;
using MochaCore.Dispatching;
using MochaCore.Navigation;
using MochaCore.Settings;
using MochaCoreWinUI.DialogsEx;
using MochaCoreWinUI.Dispatching;
using MochaCoreWinUI.Navigation;
using MochaCoreWinUI.Settings;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using ViewModels;
using ViewModels.DialogsVMs;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using WinUiApplication.Dialogs;
using WinUiApplication.Pages;

namespace WinUiApplication
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? _mainWindow;

        /// <summary>
        /// Initializes the singleton application object. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user. Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _mainWindow = new MainWindow();

            NavigationManager.AddModule(ViewModels.Pages.BlankPage1.Id, () => new NavigationModule(new BlankPage1(), new BlankPage1ViewModel()));
            NavigationManager.AddModule(ViewModels.Pages.PeoplePage.Id, () => new NavigationModule(new PeoplePage(), new PeoplePageViewModel()));
            NavigationManager.AddModule(ViewModels.Pages.BlankPage3.Id, () => new NavigationModule(new BlankPage3(), new BlankPage3ViewModel()));
            NavigationManager.AddModule(ViewModels.Pages.SettingsPage.Id, () => new NavigationModule(new SettingsPage(), new SettingsPageViewModel()));
            NavigationManager.AddModule(ViewModels.Pages.EditPersonPage.Id, () => new NavigationModule(new EditPersonPage(), new EditPersonPageViewModel()));

            DialogManager.DefineDialog(ViewModels.Dialogs.MoreInfoDialog.ID, () => new StandardMessageDialogModule(_mainWindow));
            DialogManager.DefineDialog(ViewModels.Dialogs.EditPictureDialog.ID, () => new ContentDialogModule(_mainWindow, new EditPictureDialog(), new EditPictureDialogViewModel()));
            DialogManager.DefineDialog(ViewModels.Dialogs.SelectFileDialog.ID, () => new OpenFileDialogModule(_mainWindow));

            SettingsManager.Register(ApplicationSettings.SettingsName, new ApplicationSettingsSectionProvider<ApplicationSettings>("appSettings"));

            BehaviourManager.Record("GetLocalAppFolderPath", (object o) => ApplicationData.Current.LocalFolder.Path);

            DispatcherManager.SetMainThreadDispatcher(new WinUIDispatcher(_mainWindow));

            _mainWindow.Activate();
        }
    }
}
