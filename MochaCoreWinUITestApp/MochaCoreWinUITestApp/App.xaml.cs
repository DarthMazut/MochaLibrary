using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using MochaCore.Dialogs;
using MochaCore.Navigation;
using MochaCoreWinUI.Dialogs;
using MochaCoreWinUI.Navigation;
using MochaCoreWinUITestApp.ViewModels;
using MochaCoreWinUITestApp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MochaCoreWinUITestApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? _window;

        /// <summary>
        /// Initializes the singleton application object. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();

            NavigationManager.AddModule(Pages.Page1.Id, () => new NavigationModule(new Page1(), new Page1ViewModel()));
            NavigationManager.AddModule(Pages.Page2.Id, () => new NavigationModule(new Page2(), new Page2ViewModel()));

            DialogManager.DefineDialog("TestDialog1", () =>
            {
                ContentDialogModule dialogModule = new ContentDialogModule(_window);
                dialogModule.DialogPlacementDelegate = (w, d) =>
                {
                    StackPanel sp = new StackPanel();
                    sp.Children.Add(d);
                    (w.Content as Grid).Children.Add(sp);
                };
                return dialogModule;
            });

            DialogManager.DefineDialog("OpenFileDialog", () => new FileDialogModule(_window, new FileOpenPicker()));

            _window.Activate();
        }
    }
}
