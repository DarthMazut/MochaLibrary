using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    public interface IOnNavigatingFromAsync
    {
        public Task OnNavigatingFromAsync(OnNavigatingFromEventArgs e);
    }
}
