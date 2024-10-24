﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelsX.Dialogs;

namespace ViewModelsX.Pages.Navigation
{
    public partial class NavigationPageViewModel : ObservableObject, INavigationParticipant, IOnNavigatedTo, IOnNavigatedFrom
    {
        private readonly INavigationService _internalNavigationService;
        private readonly IRemoteNavigator _proxyInternalNavigator;

        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        public NavigationPageViewModel()
        {
            _internalNavigationService = NavigationServices.InternalNavigationService;
            _proxyInternalNavigator = MochaCore.Navigation.Navigator.CreateProxy(NavigationServices.InternalNavigationServiceId, this);
        }

        public void OnNavigatedTo(OnNavigatedToEventArgs e)
        {
            _internalNavigationService.CurrentModuleChanged += InternalNavigationRequested;
            _internalNavigationService.Initialize();
        }

        public void OnNavigatedFrom(OnNavigatedFromEventArgs e)
        {
            _internalNavigationService.CurrentModuleChanged -= InternalNavigationRequested;
            _internalNavigationService.Uninitialize();
        }

        [ObservableProperty]
        private string _logText = string.Empty;

        [ObservableProperty]
        private IList<StackItemWrapper> _navigationStack = [];

        [ObservableProperty]
        private object? _internalNavigationContent;

        [RelayCommand]
        private async Task GoToPage1()
        {
            await PromptNavigationResult(await _proxyInternalNavigator.NavigateAsync("InternalPage1"));
        }

        [RelayCommand]
        private async Task GoToPage2()
        {
            await PromptNavigationResult(await _proxyInternalNavigator.NavigateAsync("InternalPage2"));
        }

        [RelayCommand]
        private async Task GoToPage3()
        {
            await PromptNavigationResult(await _proxyInternalNavigator.NavigateAsync("InternalPage3"));
        }

        [RelayCommand]
        private async Task GoBack()
        {
            await PromptNavigationResult(await _proxyInternalNavigator.NavigateBackAsync());
        }

        [RelayCommand]
        private async Task GoForward()
        {
            await PromptNavigationResult(await _proxyInternalNavigator.NavigateForwardAsync());
        }

        [RelayCommand]
        private void ClearLog() => LogText = string.Empty;

        private void InternalNavigationRequested(object? sender, CurrentNavigationModuleChangedEventArgs e)
        {
            NavigationStack = _internalNavigationService.NavigationHistory
                .Select(i => new StackItemWrapper(
                    i.Module.Id,
                    i == _internalNavigationService.CurrentItem,
                    i.IsModalOrigin))
                .ToList();
            InternalNavigationContent = e.CurrentModule.View;
        }

        private async Task PromptNavigationResult(NavigationResultData result)
        {
            using ICustomDialogModule<StandardMessageDialogProperties> dialogModule = AppDialogs.StandardMessageDialog.Module;
            dialogModule.Properties = new StandardMessageDialogProperties()
            {
                Title = "Navigation result",
                Message = result.Result.ToString()
            };
            await dialogModule.ShowModalAsync(Navigator.Module.View);
        }

        private void LogNavigationResult(NavigationResultData result) => Log($"Navigation result: {result.Result}");

        private void Log(string msg) => LogText += msg + Environment.NewLine;

        private async Task SafeExecute(Func<Task> asyncAction)
        {
            try
            {
                await asyncAction.Invoke();
            }
            catch (Exception ex) // Do not catch general exceptions!
            {
                using ICustomDialogModule<StandardMessageDialogProperties> dialogModule = AppDialogs.StandardMessageDialog.Module;
                dialogModule.Properties = new StandardMessageDialogProperties()
                {
                    Title = "Something went wrong ☠",
                    Message = ex.ToString(),
                    ConfirmationButtonText = "I'll stay calm"
                };

                await dialogModule.ShowModalAsync(Navigator.Module.View);
            }
        }
    }

    public record StackItemWrapper(string Id, bool IsCurrent, bool IsModalOrigin);
}
