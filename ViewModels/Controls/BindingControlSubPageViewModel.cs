using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Controls
{
    public partial class BindingControlSubPageViewModel : ObservableObject, IBindingTargetController
    {
        [ObservableProperty]
        private string _value = string.Empty;

        public event EventHandler<BindingTargetControlRequestedEventArgs>? ControlRequested;

        partial void OnValueChanged(string value)
        {
            ControlRequested?.Invoke(this, BindingTargetControlRequestedEventArgs.SetProperty("Value", value));
        }

        [RelayCommand]
        private void Click()
        {
            ControlRequested?.Invoke(this, BindingTargetControlRequestedEventArgs.InvokeCommand("Command", Value));
        }
    }
}
