using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Windows.AppLifecycle;
using Microsoft.Windows.AppNotifications;
using MochaCore.Behaviours;
using MochaCore.Dialogs;
using MochaCore.Dispatching;
using MochaCore.Events;
using MochaCore.Navigation;
using MochaCore.Notifications;
using MochaCore.Settings;
using MochaCore.Windowing;
using MochaWinUI.Dialogs;
using MochaWinUI.Dispatching;
using MochaWinUI.Events;
using MochaWinUI.Navigation;
using MochaWinUI.Notifications;
using MochaWinUI.Notifications.Extensions;
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
using ViewModelsX.Pages.Behaviours;
using ViewModelsX.Pages.Dialogs;
using ViewModelsX.Pages.Dispatching;
using ViewModelsX.Pages.Notfifications;
using ViewModelsX.Pages.Notfifications.Dialogs;
using ViewModelsX.Pages.Windowing;
using ViewModelsX.Pages.Windowing.Dialogs;
using ViewModelsX.Windows;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using WinUiApplicationX.Pages;
using WinUiApplicationX.Pages.Behaviours;
using WinUiApplicationX.Pages.Dialogs;
using WinUiApplicationX.Pages.Dispatching;
using WinUiApplicationX.Pages.Notifications;
using WinUiApplicationX.Pages.Notifications.Dialogs;
using WinUiApplicationX.Pages.Windowing;
using WinUiApplicationX.Pages.Windowing.Dialogs;
using WinUiApplicationX.Utils;
using WinUiApplicationX.Windows;

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
            DispatcherManager.SetMainThreadDispatcher(new WinUIDispatcher(this));

            WindowManager.RegisterWindow(AppWindows.MainWindow.Id, () => new WindowModule(new MainWindow(), new MainWindowViewModel()));
            WindowManager.RegisterWindow(
                AppWindows.WindowingGeneralWindow.Id,
                () => new WindowModule<WindowingGeneralWindowProperties>(
                    new WindowingGeneralWindow(),
                    new WindowingGeneralWindowViewModel()));

            NavigationManager.AddNavigationService(NavigationServices.MainNavigationServiceId, new WinUiNavigationService()
                .WithModule<HomePage, HomePageViewModel>(AppPages.HomePage.Id)
                .WithModule<DialogsPage, DialogsPageViewModel>(AppPages.DialogsPage.Id)
                .WithModule<DispatchingPage, DispatchingPageViewModel>(AppPages.DispatchingPage.Id)
                .WithModule<BehavioursPage, BehavioursPageViewModel>(AppPages.BehavioursPage.Id)
                .WithModule<WindowingPage, WindowingPageViewModel>(AppPages.WindowingPage.Id)
                .WithModule<NotificationsPage, NotificationsPageViewModel>(AppPages.NotificationsPage.Id)
                .WithModule<SettingsPage, SettingsPageViewModel>(AppPages.SettingsPage.Id)
                .WithInitialId(AppPages.HomePage.Id));

            DialogManager.RegisterDialog(AppDialogs.StandardMessageDialog.Id, () => new StandardMessageDialogModule());
            DialogManager.RegisterDialog(AppDialogs.CreateDialogDialog.Id, () => new ContentDialogModule<CreateDialogDialogProperties>(new CreateDialogDialog(), new CreateDialogDialogViewModel()));
            DialogManager.RegisterDialog(AppDialogs.SystemSaveDialog.Id, () => new SaveFileDialogModule());
            DialogManager.RegisterDialog(AppDialogs.SystemOpenDialog.Id, () => new OpenFileDialogModule());
            DialogManager.RegisterDialog(AppDialogs.SystemBrowseDialog.Id, () => new BrowseFolderDialogModule());
            DialogManager.RegisterDialog(AppDialogs.StandardOpenFileDialog.Id, () => new OpenFileDialogModule());
            DialogManager.RegisterDialog(AppDialogs.AddSelectableItemDialog.Id, () => new ContentDialogModule<AddSelectableItemDialogProperties>(new AddSelectableItemDialog(), new AddSelectableItemDialogViewModel()));
            DialogManager.RegisterDialog(AppDialogs.OpenWindowPropertiesDialog.Id, () => new ContentDialogModule<WindowingPageOpenWindowDialogProperties>(new WindowingPageOpenWindowDialog(), new WindowingPageOpenWindowDialogViewModel()));

            BehaviourManager.Record<TaskbarProgressData>("SetTaskBar", tpd =>
            {
                IWindowModule mainWindow = (IWindowModule)WindowManager.GetCreatedModules("MainWindow").First();
                TaskbarProgressManager.SetTaskbarState((Window)mainWindow.View, tpd);
            });

            SettingsManager.Register("Settings", new ApplicationSettingsSectionProvider<PizzaRecipe>());

            NotificationManager.RegisterNotification("GeneralNotification", () => new WinUiGeneralNotification("GeneralNotification"));

            IWindowModule mainWindow = AppWindows.MainWindow.Module;
            AppEventManager.IncludeEventProvider("AppClosing", new AppClosingEventProvider((Window)mainWindow.View));

            mainWindow.Open();

            WinUiNotification.ReportNotificationLaunch();
        }
    }
}
