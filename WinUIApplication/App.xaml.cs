using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using MochaCore.Navigation;
using MochaCoreWinUI.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ViewModels;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUiApplication.Pages;

namespace WinUiApplication
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window _mainWindow;

        /// <summary>
        /// Initializes the singleton application object. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();

            NavigationManager.AddModule(ViewModels.Pages.BlankPage1.Id, () => new NavigationModule(new BlankPage1(), new BlankPage1ViewModel()));
            NavigationManager.AddModule(ViewModels.Pages.BlankPage2.Id, () => new NavigationModule(new BlankPage2(), new BlankPage2ViewModel()));
            NavigationManager.AddModule(ViewModels.Pages.BlankPage3.Id, () => new NavigationModule(new BlankPage3(), new BlankPage3ViewModel()));
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user. Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _mainWindow = new MainWindow();
            _mainWindow.Activate();
        }
    }
}
