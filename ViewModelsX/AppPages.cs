using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX
{
    public static class AppPages
    {
        private static Dictionary<string, AppPage> _pages = new()
        {
            {
                nameof(HomePage), new AppPage(nameof(HomePage))
                {
                    Name = "Home",
                    Glyph = "\uE80F",
                    IsMenuPage = true
                }
            },
            {
                nameof(DialogsPage), new AppPage(nameof(DialogsPage))
                {
                    Name = "Dialogs",
                    Glyph = "\uE78B",
                    IsMenuPage = true
                }
            },
            {
                nameof(BehavioursPage), new AppPage(nameof(BehavioursPage))
                {
                    Name = "Behaviours",
                    Glyph = "\uE8AF",
                    IsMenuPage = true
                }
            },
            {
                nameof(SettingsPage), new AppPage(nameof(SettingsPage))
                {
                    Name = "Settings",
                    Glyph = "\uE713",
                    IsMenuPage = true,
                    MenuPlacement = MenuPlacement.Footer
                }
            }
        };

        public static AppPage HomePage => _pages[nameof(HomePage)];

        public static AppPage DialogsPage => _pages[nameof(DialogsPage)];

        public static AppPage BehavioursPage => _pages[nameof(BehavioursPage)];

        public static AppPage SettingsPage => _pages[nameof(SettingsPage)];

        public static AppPage GetById(string id) => _pages[id];

        public static IReadOnlyCollection<AppPage> AsCollection() => [.. _pages.Values];

        public static IReadOnlyCollection<AppPage> GetMenuPages() => _pages.Values.Where(p => p.IsMenuPage).ToImmutableList();

        public static IReadOnlyCollection<AppPage> GetMenuPages(MenuPlacement placement)
            => _pages.Values.Where(p => p.IsMenuPage && p.MenuPlacement == placement).ToImmutableList();
    }

    public record AppPage(string Id)
    {
        public string Name { get; init; } = string.Empty;

        public string Glyph { get; init; } = string.Empty;

        public bool IsMenuPage { get; init; }

        public MenuPlacement MenuPlacement { get; init; }

        public bool IsFullScreen { get; init; }
    }

    public enum MenuPlacement
    {
        Top,
        Footer
    }
}
