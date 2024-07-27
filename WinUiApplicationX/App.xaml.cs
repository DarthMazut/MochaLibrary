using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using MochaCore.Dialogs;
using MochaCore.Events;
using MochaCore.Navigation;
using MochaCore.Settings;
using MochaCore.Windowing;
using MochaWinUI.Dialogs;
using MochaWinUI.Events;
using MochaWinUI.Navigation;
using MochaWinUI.Settings;
using MochaWinUI.Windowing;
using ModelX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ViewModelsX;
using ViewModelsX.Dialogs;
using ViewModelsX.Pages;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUiApplicationX.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiApplicationX
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            WindowManager.RegisterWindow("MainWindow", () => new WindowModule(new MainWindow(), new MainWindowViewModel()));

            NavigationManager.AddNavigationService(NavigationServices.MainNavigationServiceId, new WinUiNavigationService()
                .WithModule<HomePage, HomePageViewModel>(AppPages.HomePage.Id)
                .WithModule<DialogsPage, DialogsPageViewModel>(AppPages.DialogsPage.Id)
                .WithModule<SettingsPage, SettingsPageViewModel>(AppPages.SettingsPage.Id)
                .WithInitialId(AppPages.HomePage.Id));

            DialogManager.RegisterDialog(AppDialogs.StandardMessageDialog.Id, () => new StandardMessageDialogModule());

            SettingsManager.Register("Settings", new ApplicationSettingsSectionProvider<PizzaRecipe>());

            IWindowModule mainWindow = WindowManager.RetrieveWindow("MainWindow");
            AppEventManager.IncludeEventProvider("AppClosing", new AppClosingEventProvider((Window)mainWindow.View));
            mainWindow.Open();
        }
    }
}
