using Microsoft.UI.Xaml.Controls;
using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCoreWinUITestApp
{
    public class PageInfo
    {
        public PageInfo(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public string Name { get; set; } = string.Empty;

        public string? FontIcon { get; set; }

        public INavigationModule NavigationModule => NavigationManager.FetchModule(Id);
    }
}
