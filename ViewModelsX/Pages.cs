using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX
{
    public static class Pages
    {
        private static Dictionary<string, AppPage> _pages = new()
        {
            { 
                nameof(HomePage), new AppPage(nameof(HomePage)) 
                {
                    Name = "Home Page",
                    Glyph = "\uE80F",
                    IsMenuPage = true
                }
            },
        };

        public static AppPage HomePage => _pages[nameof(HomePage)];

        public static IReadOnlyCollection<AppPage> AsCollection() => [.. _pages.Values];

        public static IReadOnlyCollection<AppPage> GetMenuPages() => _pages.Values.Where(p => p.IsMenuPage).ToImmutableList();
    }

    public record AppPage(string Id)
    {
        public string Name { get; init; } = string.Empty;

        public string Glyph { get; init; } = string.Empty;

        public bool IsMenuPage { get; init; } = false;

        public bool IsFullScreen { get; init; } = false;
    }
}
