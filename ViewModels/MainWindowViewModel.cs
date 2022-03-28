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

namespace ViewModels;

public class MainWindowViewModel : BindableBase, INavigatable
{
    private object? _frameContent;
    private AsyncProperty<Page?> _selectedPage;

    public MainWindowViewModel()
    {
        NavigationServices.MainNavigationService.NavigationRequested += HandleNavigationRequest;
        Navigator = new Navigator(this, NavigationServices.MainNavigationService);
        _selectedPage = new(this, nameof(SelectedPage))
        {
            InitialValue = NavigationPages.FirstOrDefault(),
            PropertyChangedCallback = SelectedPageChanged
        };
    }

    private async void SelectedPageChanged(AsyncPropertyChangedEventArgs<Page?> e)
    {
        if (SelectedPage is not null)
        {
            NavigationResultData navigationResult = await Navigator.NavigateAsync(NavigationManager.FetchModule(SelectedPage.Id));
            if (navigationResult.Result != NavigationResult.Succeed)
            {
                await Task.Yield();
                SelectedPage = e.PreviousValue;
            }
        }
    }

    private void HandleNavigationRequest(object? sender, NavigationData e)
    {
        FrameContent = e.RequestedModule.View;
        SelectedPage = GetPageFromModule(e.RequestedModule);
    }

    public Navigator Navigator { get; }

    public IList<Page> NavigationPages { get; } = new List<Page> { Pages.BlankPage1, Pages.BlankPage2 };

    public object? FrameContent
    {
        get => _frameContent;
        set => SetProperty(ref _frameContent, value);
    }

    public Page? SelectedPage
    {
        get => _selectedPage.Get();
        set => _selectedPage.Set(value);
    }

    private void SelectedPageChangedCallback(object result, AsyncPropertyChangedEventArgs<Page?> e)
    {
        if (result is not null)
        {
            SelectedPage = result as Page;
        }
    }

    private Page? GetPageFromModule(INavigationModule requestedModule)
    {
        foreach (Page page in NavigationPages)
        {
            if (requestedModule.Equals(NavigationManager.FetchModule(page.Id)))
            {
                return page;
            }
        }

        return null;
    }
}
