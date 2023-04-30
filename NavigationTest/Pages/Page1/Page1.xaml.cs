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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NavigationTest.Pages.Page1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page1 : Page
    {
        public Page1()
        {
            this.InitializeComponent();
        }

        private async void em_ShowNotification(object sender, RoutedEventArgs e)
        {
            // Requires Microsoft.Toolkit.Uwp.Notifications NuGet package version 7.0 or greater
            await Task.Delay(5000);
            new ToastContentBuilder()
                .AddText("Andrew sent you a picture")
                .AddText("Check this out, The Enchantments in Washington!")
                .AddInlineImage(new Uri("https://yt3.googleusercontent.com/ytc/AL5GRJViZXkDPQIZJaL0aSR9SDDTILS5HUwjO7aUtn5W=s900-c-k-c0x00ffffff-no-rj"))
                .AddButton(new ToastButton()
                    .SetContent("Like")
                    .AddArgument("action", "like")
                    .SetBackgroundActivation())
                .Show();
            (Application.Current as App).MainWindow.Activate();
        }
    }
}
