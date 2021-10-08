using MochaCore.Navigation;

namespace MochaCoreWinUITestApp.ViewModels
{
    public class Page2ViewModel : INavigatable
    {
        public Navigator Navigator { get; }

        public Page2ViewModel()
        {
            Navigator = new(this, NavigationServices.MainNavigationService);
        }
    }
}
