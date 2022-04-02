using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels;

public static class Pages
{
    public static ApplicationPage BlankPage1 { get; } = new("BlankPage1", "Page 1 xD", "\xE703");

    public static ApplicationPage BlankPage2 { get; } = new("BlankPage2", "Page 2 xD", "\xE703");

    public static ApplicationPage BlankPage3 { get; } = new("BlankPage3", "Page 3 xD", "\xE703");

    public static ApplicationPage SettingsPage { get; } = new("SettingsPage", string.Empty, string.Empty);
}

public class ApplicationPage
{
    public ApplicationPage(string id, string name) : this(id, name, string.Empty) { }

    public ApplicationPage(string id, string name, string glyph)
    {
        Id = id;
        Name = name;
        Glyph = glyph;
    }

    public string Id { get; }

    public string Name { get; }

    public string Glyph { get; }

    public INavigationModule GetNavigationModule() => NavigationManager.FetchModule(Id);
}

