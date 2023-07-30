namespace ViewModels;

public static class Pages
{
    public static ApplicationPage BlankPage1 { get; } = new("BlankPage1")
    {
        Name = "Page 1 xD",
        Glyph = "\xE703"
    };

    public static ApplicationPage PeoplePage { get; } = new("PeoplePage")
    {
        Name = "People",
        Glyph = "\xE716"
    };

    public static ApplicationPage BlankPage3 { get; } = new("BlankPage3")
    {
        Name = "Page 3 xD",
        Glyph = "\xE703",
        IsFullScreen = true
    };

    public static ApplicationPage SettingsPage { get; } = new("SettingsPage")
    { 
    };

    public static ApplicationPage EditPersonPage { get; } = new("EditPersonPage")
    {
        IsFullScreen = true
    };

    public static IEnumerable<ApplicationPage> AsCollection()
    {
        return new List<ApplicationPage>()
        {
            BlankPage1,
            PeoplePage,
            BlankPage3,
            SettingsPage,
            EditPersonPage
        };
    }
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

    public bool IsFullScreen { get; init; } = false;
}

