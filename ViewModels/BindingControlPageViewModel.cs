using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Navigation;
using MochaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public partial class BindingControlPageViewModel : ObservableObject, INavigationParticipant, IBindingTargetController
    {
        [ObservableProperty]
        private BindingControlPageState _selectedState = BindingControlPageState.State1;

        [ObservableProperty]
        private int _clickCount;

        public BindingControlPageState[] AvailableStates { get; } = 
        { 
            BindingControlPageState.State1,
            BindingControlPageState.State2 
        };

        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        public event EventHandler<BindingTargetControlRequestedEventArgs>? ControlRequested;

        partial void OnSelectedStateChanged(BindingControlPageState value)
        {
            ControlRequested?.Invoke(this, BindingTargetControlRequestedEventArgs.SetVisualState(value.ToString()));
        }

        [RelayCommand]
        private void PlayAnimation()
        {
            ControlRequested?.Invoke(this, BindingTargetControlRequestedEventArgs.PlayAnimation("Animation"));
        }

        [RelayCommand]
        private void SubPageClick()
        {
            ClickCount++;
        }

        public enum BindingControlPageState
        {
            State1,
            State2
        }
    }
}
