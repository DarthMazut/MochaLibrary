using MochaCoreWPFTestApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


namespace MochaCoreWPFTestApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        //private OperationResult<bool> VerifyFunc(string input)
        //{
        //    return new OperationResult<bool>
        //    {
        //        Data = !(DataContext as MainWindowViewModel)!.IsError,
        //        Message = xe_SearchBar.ErrorStr,
        //        ResultType = ResultType.Success
        //    };
        //}
    }
}
