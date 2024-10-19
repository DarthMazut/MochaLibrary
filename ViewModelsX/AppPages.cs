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
                nameof(NavigationPage), new AppPage(nameof(NavigationPage))
                {
                    Name = "Navigation",
                    Glyph = "\uE786",
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
                nameof(DispatchingPage), new AppPage(nameof(DispatchingPage))
                {
                    Name = "Dispatching",
                    Glyph = "\xE717",
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
                nameof(WindowingPage), new AppPage(nameof(WindowingPage))
                {
                    Name = "Windowing",
                    Glyph = "\uE737",
                    IsMenuPage = true
                }
            },
            {
                nameof(NotificationsPage), new AppPage(nameof(NotificationsPage))
                {
                    Name = "Notifications",
                    Glyph = "\uE91C",
                    IsMenuPage = true
                }
            },
            {
                nameof(SandboxPage), new AppPage(nameof(SandboxPage))
                {
                    Name = "Sandbox",
                    Glyph = "\uE822",
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

        public static AppPage NavigationPage => _pages[nameof(NavigationPage)];

        public static AppPage DialogsPage => _pages[nameof(DialogsPage)];

        public static AppPage DispatchingPage => _pages[nameof(DispatchingPage)];

        public static AppPage BehavioursPage => _pages[nameof(BehavioursPage)];

        public static AppPage WindowingPage => _pages[nameof(WindowingPage)];

        public static AppPage NotificationsPage => _pages[nameof(NotificationsPage)];

        public static AppPage SandboxPage => _pages[nameof(SandboxPage)];

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
