using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using MochaCore.Dialogs;
using MochaCore.Windowing;
using MochaWinUI.Dialogs;
using MochaWinUI.Windowing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ViewModelsX;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
            IWindowModule mainWindow = WindowManager.RetrieveWindow("MainWindow");

            DialogManager.DefineDialog("CustomDialog", () => new ContentDialogModule<MyDialogProperties>(
                new ContentDialog(),
                new DialogViewModel())
            {
                FindParent = (o) => (mainWindow.View as MainWindow).Content.XamlRoot
            });
            DialogManager.DefineDialog("StdDialog", () => new StandardMessageDialogModule());
            DialogManager.DefineDialog("OpenDialog", () => new OpenFileDialogModule(mainWindow.View as Window));

            mainWindow.Open();
        }

        
    }
}
