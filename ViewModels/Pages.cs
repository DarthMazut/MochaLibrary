using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels;

public static class Pages
{
    public static Page BlankPage1 { get; } = new("BlankPage1", "Page 1 xD");

    public static Page BlankPage2 { get; } = new("BlankPage2", "Page 2 xD");

    public static Page BlankPage3 { get; } = new("BlankPage3", "Page 3 xD");
}

public class Page
{
    public Page(string id, string name)
    {
        Id = id;
        MenuName = name;
    }

    public string Id { get; }

    public string MenuName { get; }

    public INavigationModule Module => NavigationManager.FetchModule(Id);
}

