// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using MochaCore.NavigationEx;
using MochaCore.NavigationEx.Extensions;
using MochaWinUI.NavigationEx;
using NavigationTest.Pages.HomePage;
using NavigationTest.Pages.InnerModalPage;
using NavigationTest.Pages.ModalPage;
using NavigationTest.Pages.Page1;
using NavigationTest.Pages.Page2;
using NavigationTest.Pages.Page3;
using NavigationTest.Pages.SettingsPage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NavigationTest
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window _window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        public Window MainWindow => _window;

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            INavigationService mainNavigationService = NavigationManager.AddNavigationService(
                "MainNavigationService",
                new WinUiNavigationService()
                    .WithModule<HomePage, HomePageViewModel>(AppPages.HomePage.Id)
                    .WithModule<Page1, Page1ViewModel>(AppPages.Page1.Id)
                    .WithModule<Page2, Page2ViewModel>(AppPages.Page2.Id, new NavigationModuleLifecycleOptions()
                    {
                        PreferCache = true
                    })
                    .WithModule<Page3, Page3ViewModel>(AppPages.Page3.Id)
                    .WithModule<ModalPage, ModalPageViewModel>(AppPages.ModalPage.Id)
                    .WithModule<InnerModalPage, InnerModalPageViewModel>("InnerModalPage")
                    .WithModule<SettingsPage, SettingsPageViewModel>(AppPages.SettingsPage.Id)
                    .WithInitialId(AppPages.HomePage.Id)
                );

            _window = new MainWindow();
            _window.Activate();

            _ = mainNavigationService.Initialize();
        }
    }
}
