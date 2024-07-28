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
            SetupFormatter(xe_YeastNumberBox, 0.05);
            SetupFormatter(xe_SaltNumberBox, 0.1);
        }

        // This won't work when specified in XAML style. Most likely XAML parser is broken here:
        // https://github.com/microsoft/microsoft-ui-xaml/issues/9365
        private void SetupFormatter(NumberBox numberBox, double increment)
        {
            numberBox.NumberFormatter = new DecimalFormatter()
            {
                IntegerDigits = 1,
                FractionDigits = 2,
                NumberRounder = new IncrementNumberRounder()
                {
                    Increment = increment,
                    RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp
                }
            };
        }
    }
}
