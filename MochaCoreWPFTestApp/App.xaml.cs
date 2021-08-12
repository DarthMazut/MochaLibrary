using MochaCore.Navigation;
using MochaCoreWPF.Navigation;
using MochaCoreWPFTestApp.ViewModels;
using MochaCoreWPFTestApp.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MochaCoreWPFTestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            NavigationManager.AddModule(Pages.Page1Id, () => new NavigationModule(new Page1(), new Page1ViewModel()));
            NavigationManager.AddModule(Pages.Page2Id, () => new NavigationModule(new Page2(), new Page2ViewModel()));
        }
    }
}
