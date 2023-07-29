using Microsoft.UI.Xaml;
using MochaCore.Behaviours;
using MochaCore.Dialogs;
using MochaCore.Dispatching;
using MochaCore.NavigationEx;
using MochaCore.Settings;
using MochaWinUI.Dialogs;
using MochaWinUI.Dispatching;
using MochaWinUI.NavigationEx;
using MochaWinUI.Settings;
using Model;
using ViewModels;
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

            INavigationService mainNavigationService = NavigationManager.AddNavigationService(
                NavigationServices.MainNavigationServiceId,
                new WinUiNavigationService()
                    .WithModule<BlankPage1, BlankPage1ViewModel>()
                    .WithModule<PeoplePage, PeoplePageViewModel>()
                    .WithModule<BlankPage3, BlankPage3ViewModel>()
                    .WithModule<SettingsPage, SettingsPageViewModel>()
                    .WithModule<EditPersonPage, EditPersonPageViewModel>()
                    .WithInitialId(ViewModels.Pages.BlankPage1.Id));

            mainNavigationService.Initialize();

            DialogManager.DefineDialog(ViewModels.Dialogs.MoreInfoDialog.ID, () => new StandardMessageDialogModule(_mainWindow));
            DialogManager.DefineDialog(ViewModels.Dialogs.EditPictureDialog.ID, () => new ContentDialogModule(_mainWindow, new EditPictureDialog()));
            DialogManager.DefineDialog(ViewModels.Dialogs.SelectFileDialog.ID, () => new OpenFileDialogModule(_mainWindow) { DialogIdentifier = "test"});

            SettingsManager.Register(ApplicationSettings.SettingsName, new ApplicationSettingsSectionProvider<ApplicationSettings>("appSettings"));

            BehaviourManager.Record("GetLocalAppFolderPath", (object o) => ApplicationData.Current.LocalFolder.Path);

            DispatcherManager.SetMainThreadDispatcher(new WinUIDispatcher(_mainWindow));
             
            _mainWindow.Activate();
        }
    }
}
