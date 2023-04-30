using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    public interface IOnNavigatedFromAsync
    {
        public Task OnNavigatedFromAsync(OnNavigatedFromEventArgs e);
    }
}
