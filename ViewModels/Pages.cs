namespace ViewModels;

public static class Pages
{
    private static List<ApplicationPage> _pages = new()
    {
        new(nameof(BlankPage1))
        {
            Name = "Page 1 xD",
            IsMenuPage = true,
            Glyph = "\xE703"
        },

        new(nameof(PeoplePage))
        {
            Name = "People",
            IsMenuPage = true,
            Glyph = "\xE716"
        },

        new(nameof(BlankPage3))
        {
            Name = "Page 3 xD",
            IsMenuPage = true,
            Glyph = "\xE703",
            IsFullScreen = true
        },

        new(nameof(BindingControlPage))
        {
            Name = "Binding Control",
            IsMenuPage = true,
            Glyph = "\xE71B"
        },

        new(nameof(SettingsPage))
        {
        },

        new(nameof(EditPersonPage))
        {
            IsFullScreen = true
        },

        new(nameof(WindowingPage))
        {
            Name = "Windowing Page",
            Glyph = "\xE8A7",
            IsMenuPage = true,
        },

        new(nameof(DispatcherPage))
        {
            Name = "Dispatcher Page",
            Glyph = "\xE717",
            IsMenuPage = true,
        },

        new(nameof(NotificationsPage))
        {
            Name = "Notifications Page",
            Glyph = "\xE8BD",
            IsMenuPage = true,
        }
    };

    public static ApplicationPage BlankPage1 => _pages.First(p => p.Id == nameof(BlankPage1));

    public static ApplicationPage PeoplePage => _pages.First(p => p.Id == nameof(PeoplePage));

    public static ApplicationPage BlankPage3 => _pages.First(p => p.Id == nameof(BlankPage3));

    public static ApplicationPage BindingControlPage => _pages.First(p => p.Id == nameof(BindingControlPage));

    public static ApplicationPage SettingsPage => _pages.First(p => p.Id == nameof(SettingsPage));

    public static ApplicationPage EditPersonPage => _pages.First(p => p.Id == nameof(EditPersonPage));

    public static ApplicationPage WindowingPage => _pages.First(p => p.Id == nameof(WindowingPage));

    public static ApplicationPage DispatcherPage => _pages.First(p => p.Id == nameof(DispatcherPage));

    public static ApplicationPage NotificationsPage => _pages.First(p => p.Id == nameof(NotificationsPage));

    public static IList<ApplicationPage> AsCollection() => _pages;

    public static IList<ApplicationPage> GetMenuPages() => _pages.Where(p => p.IsMenuPage).ToList();
}

public class ApplicationPage
{
    public ApplicationPage(string id)
    {
        Id = id;
    }

    public string Id { get; }

    public string Name { get; init; } = string.Empty;

    public string Glyph { get; init; } = string.Empty;

    public bool IsMenuPage { get; init; } = false;

    public bool IsFullScreen { get; init; } = false;
}

