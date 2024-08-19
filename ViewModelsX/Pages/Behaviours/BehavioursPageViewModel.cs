using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Behaviours;
using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Pages.Behaviours
{
    public partial class BehavioursPageViewModel : ObservableObject, INavigationParticipant
    {
        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        [ObservableProperty]
        private TaskbarProgressState _progressState;

        [ObservableProperty]
        private double _progressValue;

        [RelayCommand]
        private void ApplyState()
        {
            BehaviourManager.Recall<TaskbarProgressData>("SetTaskBar").Execute(new TaskbarProgressData(ProgressState, ProgressValue));
        }

        public string Title => "Behaviours page 😏";
    }

    public record struct TaskbarProgressData(TaskbarProgressState State, double Value);

    public enum TaskbarProgressState
    {
        NoProgress = 0,
        Indeterminate = 1,
        Normal = 2,
        Error = 4,
        Paused = 8
    }
}
