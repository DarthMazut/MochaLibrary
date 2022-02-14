using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
using MochCoreWPF.DialogsEx;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DialogManager.DefineDialog("MessageBox", () => new StandardMessageDialogModule(MainWindow));
        }
    }
}
