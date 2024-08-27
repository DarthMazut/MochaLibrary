﻿using CommunityToolkit.Mvvm.ComponentModel;
using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Pages.Notfifications
{
    public class NotificationsPageViewModel : ObservableObject, INavigationParticipant, IOnNavigatedTo
    {
        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        public NotificationsPageViewModel()
        {
            GeneralTabVM = new(Navigator);
        }

        public NotificationsGeneralTabViewModel GeneralTabVM { get; }

        public void OnNavigatedTo(OnNavigatedToEventArgs e) => GeneralTabVM.OnNavigatedTo(e);
    }
}
