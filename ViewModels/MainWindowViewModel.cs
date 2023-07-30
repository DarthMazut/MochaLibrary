using MochaCore.NavigationEx;
using MochaCore.Utils;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModels.Wrappers;

namespace ViewModels;

public class MainWindowViewModel : BindableBase
{
    private object? _frameContent;
    private object? _fullScreenContent;
    private ApplicationPage? _selectedPage;
    private bool _isSettigsInvoked;
    private bool _isFullScreen;

    private DelegateCommand<NavigationInvokedDetails>? _navigationItemInvokedCommand;

    public MainWindowViewModel()
    {
        NavigationServices.MainNavigationService.CurrentModuleChanged += HandleNavigationRequest;
        NavigationServices.MainNavigationService.Initialize();
    }

    public IReadOnlyList<ApplicationPage> NavigationPages { get; } = new List<ApplicationPage>
    {
        Pages.BlankPage1,
        Pages.PeoplePage,
        Pages.BlankPage3
    };

    public bool IsSettingsInvoked
    {
        get => _isSettigsInvoked;
        set => SetProperty(ref _isSettigsInvoked, value);
    }

    public bool IsFullScreen
    {
        get => _isFullScreen;
        set => SetProperty(ref _isFullScreen, value);
    }

    public object? FrameContent
    {
        get => _frameContent;
        set => SetProperty(ref _frameContent, value);
    }

    public object? FullScreenContent
    {
        get => _fullScreenContent;
        set => SetProperty(ref _fullScreenContent, value);
    }

    public ApplicationPage? SelectedPage
    {
        get => _selectedPage;
        set => SetProperty(ref _selectedPage, value);
    }

    public DelegateCommand<NavigationInvokedDetails> NavigationItemInvokedCommand => _navigationItemInvokedCommand ?? (_navigationItemInvokedCommand = new DelegateCommand<NavigationInvokedDetails>(NavigationItemInvoked));

    private async void NavigationItemInvoked(NavigationInvokedDetails e)
    {
        IRemoteNavigator remoteNavigator = Navigator.CreateProxy(NavigationServices.MainNavigationServiceId, this);

        if (e.InvokedPage is not null)
        {
            NavigationResultData navigationResult = await remoteNavigator.NavigateAsync(e.InvokedPage.Id);
            if (navigationResult.Result != NavigationResult.Succeed)
            {
                await Task.Yield();
                SelectedPage = Pages.AsCollection().FirstOrDefault(p => p.Id == NavigationServices.MainNavigationService.CurrentModule.Id);
            }

        }
        else
        {
            if(e.IsSettingsInvoked)
            {
                await remoteNavigator.NavigateAsync(Pages.SettingsPage.Id);
            }
            else
            {
                // something went wrong... ;(
            }
        }
    }

    private void HandleNavigationRequest(object? sender, CurrentNavigationModuleChangedEventArgs e)
    {
        SelectedPage = Pages.AsCollection().FirstOrDefault(p => p.Id == e.CurrentModule.Id);
        IsFullScreen = SelectedPage?.IsFullScreen ?? false;
        if (IsFullScreen)
        {
            FrameContent = null;
            FullScreenContent = e.CurrentModule.View;
        }
        else
        {
            FullScreenContent = null;
            FrameContent = e.CurrentModule.View;
        }
    }
}
