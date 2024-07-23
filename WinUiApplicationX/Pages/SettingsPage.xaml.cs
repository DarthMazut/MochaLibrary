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
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization.NumberFormatting;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiApplicationX.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            //IncrementNumberRounder rounder = new IncrementNumberRounder();
            //rounder.Increment = 0.1;
            //rounder.RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp;

            //DecimalFormatter formatter = new DecimalFormatter();
            //formatter.IntegerDigits = 1;
            //formatter.FractionDigits = 2;
            //formatter.NumberRounder = rounder;
            //xe_NumberBox.NumberFormatter = formatter;

        }

        private void RestoreDefaultsClicked(object sender, RoutedEventArgs e) => xe_RestoreDefaultsFlyout.Hide();
    }
}
