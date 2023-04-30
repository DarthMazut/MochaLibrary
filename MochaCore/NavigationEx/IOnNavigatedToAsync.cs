using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    public interface IOnNavigatedToAsync
    {
        public Task OnNavigatedToAsync(OnNavigatedToEventArgs e);
    }
}
