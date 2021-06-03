using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Mocha.Navigation;
using Mocha.Dialogs;
using MochaWPFTestApp.Views;
using MochaWPFTestApp.ViewModels;
using MochaWPF;
using MochaWPFTestApp.Views.Dialogs;
using MochaWPFTestApp.ViewModels.Dialogs;
using Mocha.Settings;
using MochaWPF.Settings;
using MochaWPF.Dialogs;
using MochaWPF.Navigation;
using MochaWPFTestApp.Settings;
using Mocha.Dispatching;
using MochaWPF.Dispatching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mocha.Behaviours;
using Mocha.Dialogs.Extensions.DI;

namespace MochaWPFTestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            IServiceProvider provider = RegisterServices();

            // Behaviours

            //IBehaviourService behaviourService = new BehaviourService();
            //behaviourService.Record<object, Task<string>>("exit", (o) =>
            //{
            //    return Task.Delay(3000).ContinueWith(t => Task.FromResult("haha")).Unwrap();
            //});

            SetupNavigation(provider);
            SetupDialogs(provider);

            // Settings

            SettingsManager.Register("myCustomSettings1", new ApplicationSettingsSection<GeneralSettings>(MochaWPFTestApp.Properties.Settings.Default, "general"));

            // Dispatching

            DispatcherManager.SetMainThreadDispatcher(new WpfDispatcher(this));

            new MainWindow()
            {
                DataContext = provider.GetRequiredService<MainWindowViewModel>()
            }.Show();
        }

        private IServiceProvider RegisterServices()
        {
            IHost host = Host.CreateDefaultBuilder().ConfigureServices((_, services) =>
            {
                services.AddSingleton<IBehaviourService, BehaviourService>();
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<IDialogFactory, DialogFactory>();

                services.AddTransient<MainWindowViewModel>();
                
                services.AddTransient<Page1ViewModel>();
                services.AddTransient<Page2ViewModel>();
                services.AddTransient<Page3ViewModel>();

                services.AddTransient<MyCustomDialogViewModel>();
                
            }).Build();

            return host.Services.CreateScope().ServiceProvider;
        }

        private void SetupDialogs(IServiceProvider provider)
        {
            DialogManager.DefineDialog(DialogsIDs.MsgBoxDialog, () =>
            {
                return new StandardDialogModule(this);
            });

            DialogManager.DefineDialog(DialogsIDs.OpenDialog, () =>
            {
                return new FileDialogModule(this, new Microsoft.Win32.OpenFileDialog());
            });

            DialogManager.DefineDialog(DialogsIDs.CustomDialog1, () =>
            {
                return new CustomDialogModule(this, new MyCustomDialog(), provider.GetRequiredService<MyCustomDialogViewModel>());
            });
        }

        private void SetupNavigation(IServiceProvider provider)
        {
            NavigationManager.AddModule(PagesIDs.Page1, () =>
            {
                return new NavigationModule(
                    new Page1(),
                    provider.GetRequiredService<Page1ViewModel>());
            });

            NavigationManager.AddModule(PagesIDs.Page2, () =>
            {
                return new NavigationModule(
                    new Page2(),
                    provider.GetRequiredService<Page2ViewModel>());
            });

            NavigationManager.AddModule(PagesIDs.Page3, () =>
            {
                return new NavigationModule(
                    new Page3(),
                    provider.GetRequiredService<Page2ViewModel>());
            });
        }
    }
}
