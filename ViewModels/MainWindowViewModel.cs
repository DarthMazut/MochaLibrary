using MochaCore.Navigation;
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

public class MainWindowViewModel : BindableBase, INavigatable
{
    private object? _frameContent;
    private ApplicationPage? _selectedPage;
    private bool _isSettigsInvoked;

    private DelegateCommand<NavigationInvokedDetails> _navigationItemInvokedCommand;
    private DelegateCommand _loadedCommand;

    public MainWindowViewModel()
    {
        NavigationServices.MainNavigationService.NavigationRequested += HandleNavigationRequest;
        Navigator = new Navigator(this, NavigationServices.MainNavigationService);
    }

    public Navigator Navigator { get; }

    public IList<ApplicationPage> NavigationPages { get; } = new List<ApplicationPage> 
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

    public object? FrameContent
    {
        get => _frameContent;
        set => SetProperty(ref _frameContent, value);
    }

    public ApplicationPage? SelectedPage
    {
        get => _selectedPage;
        set => SetProperty(ref _selectedPage, value);
    }

    
    public DelegateCommand<NavigationInvokedDetails> NavigationItemInvokedCommand => _navigationItemInvokedCommand ?? (_navigationItemInvokedCommand = new DelegateCommand<NavigationInvokedDetails>(NavigationItemInvoked));
    public DelegateCommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new DelegateCommand(Loaded));

    private async void NavigationItemInvoked(NavigationInvokedDetails e)
    {
        if (e.InvokedPage is not null)
        {
            NavigationResultData navigationResult = await Navigator.NavigateAsync(e.InvokedPage.GetNavigationModule());
            if (navigationResult.Result != NavigationResult.Succeed)
            {
                await Task.Yield();
                SelectedPage = GetPageFromModule(NavigationServices.MainNavigationService.CurrentView);
            }

        }
        else
        {
            if(e.IsSettingsInvoked)
            {
                await Navigator.NavigateAsync(Pages.SettingsPage.GetNavigationModule());
            }
            else
            {
                // some thing went wrong... ;(
            }
        }
    }

    private void Loaded()
    {
        Navigator.NavigateAsync(Pages.BlankPage1.GetNavigationModule());
    }

    private void HandleNavigationRequest(object? sender, NavigationData e)
    {
        FrameContent = e.RequestedModule.View;
        SelectedPage = GetPageFromModule(e.RequestedModule);
    }

    private ApplicationPage? GetPageFromModule(INavigationModule? requestedModule)
    {
        if (requestedModule is null)
        {
            return null;
        }

        foreach (ApplicationPage page in NavigationPages)
        {
            if (requestedModule.Equals(NavigationManager.FetchModule(page.Id)))
            {
                return page;
            }
        }

        return null;
    }
}
