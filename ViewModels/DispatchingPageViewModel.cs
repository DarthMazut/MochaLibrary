using CommunityToolkit.Mvvm.Input;
using MochaCore.Dispatching;
using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public partial class DispatchingPageViewModel : INavigationParticipant
    {
        public INavigator Navigator => MochaCore.Navigation.Navigator.Create();

        [RelayCommand]
        private void TestDispatcher()
        {
            IDispatcher dispatcher = DispatcherManager.GetMainThreadDispatcher();

            Debug.WriteLine("Start TestDispacther()");
            dispatcher.RunOnMainThread(async () =>
            {
                Debug.WriteLine("Start executing async delegate");
                await Task.Delay(5000);
                //Thread.Sleep(5000);
                Debug.WriteLine("Finish executing async delegate");
            });
            Debug.WriteLine("Finish TestDispacther()");
        }
    }
}
