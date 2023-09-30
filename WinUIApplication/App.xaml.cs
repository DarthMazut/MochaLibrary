using Microsoft.UI.Xaml;
using MochaCore.Behaviours;
using MochaCore.Dialogs;
using MochaCore.Dispatching;
using MochaCore.Navigation;
using MochaCore.Settings;
using MochaCore.Windowing;
using MochaWinUI.Dialogs;
using MochaWinUI.Dispatching;
using MochaWinUI.Navigation;
using MochaWinUI.Settings;
using MochaWinUI.Windowing;
using Model;
using ViewModels;
using ViewModels.Windows;
using Windows.Storage;
using WinUiApplication.Dialogs;
using WinUiApplication.Pages;
using WinUiApplication.Windows;

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
            WindowManager.RegisterWindow("MainWindow", () => new WindowModule(new MainWindow(), new MainWindowViewModel()));
            WindowManager.RegisterWindow("TestWindow", () => new WindowModule<GenericWindowProperties>(new TestWindow(), new TestWindowViewModel()));

            INavigationService mainNavigationService = NavigationManager.AddNavigationService(
                NavigationServices.MainNavigationServiceId,
                new WinUiNavigationService()
                    .WithModule<BlankPage1, BlankPage1ViewModel>()
                    .WithModule<PeoplePage, PeoplePageViewModel>()
                    .WithModule<BlankPage3, BlankPage3ViewModel>()
                    .WithModule<SettingsPage, SettingsPageViewModel>()
                    .WithModule<EditPersonPage, EditPersonPageViewModel>()
                    .WithModule<BindingControlPage, BindingControlPageViewModel>()
                    .WithModule<WindowingPage, WindowingPageViewModel>()
                    .WithInitialId(ViewModels.Pages.BlankPage1.Id));

            IBaseWindowModule mainWindowModule = WindowManager.RetrieveWindow("MainWindow");
            _mainWindow = mainWindowModule.View as MainWindow;

            DialogManager.DefineDialog(ViewModels.Dialogs.MoreInfoDialog.ID, () => new StandardMessageDialogModule(_mainWindow));
            DialogManager.DefineDialog(ViewModels.Dialogs.EditPictureDialog.ID, () => new ContentDialogModule(_mainWindow, new EditPictureDialog()));
            DialogManager.DefineDialog(ViewModels.Dialogs.SelectFileDialog.ID, () => new OpenFileDialogModule(_mainWindow) { DialogIdentifier = "test"});

            SettingsManager.Register(ApplicationSettings.SettingsName, new ApplicationSettingsSectionProvider<ApplicationSettings>("appSettings"));

            BehaviourManager.Record("GetLocalAppFolderPath", (object o) => ApplicationData.Current.LocalFolder.Path);

            DispatcherManager.SetMainThreadDispatcher(new WinUIDispatcher(_mainWindow));

            mainWindowModule.Open();

            //_mainWindow.Activate();
        }
    }
}
