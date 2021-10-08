using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using MochaCoreWinUITestApp.ViewModels;
using MochaCoreWinUITestApp.Views;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MochaCoreWinUITestApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            if (Content is FrameworkElement bindableContent)
            {
                _viewModel = new MainWindowViewModel();
                bindableContent.DataContext = _viewModel;
            }
            else
            {
                throw new Exception("Root element must be of type FrameworkElement");
            }
        }

        private void NavigationViewOnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                return;
            }

            if (sender.SelectedItem is PageInfo pageInfo)
            {
                _viewModel.NavigationItemSelectionChangedCommand?.Execute(pageInfo);
            }
            else
            {
                throw new Exception("MenuItem was not PageInfo object!");
            } 
        }
    }
}
