using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationTest
{
    public static class AppPages
    {
        public static AppPage HomePage { get; } = new AppPage()
        {
            Id = nameof(HomePage), 
            Name = "Home Page",
            Glyph = "\uE80F"
        };

        public static AppPage Page1 { get; } = new AppPage()
        {
            Id = nameof(Page1),
            Name = "Page One",
            Glyph = "\uE7C3"
        };

        public static AppPage Page2 { get; } = new AppPage()
        {
            Id = nameof(Page2),
            Name = "Page Two",
            Glyph = "\uE7C3"
        };

        public static AppPage Page3 { get; } = new AppPage()
        {
            Id = nameof(Page3),
            Name = "Page Three",
            Glyph = "\uE7C3"
        };

        public static AppPage SettingsPage { get; } = new AppPage()
        {
            Id = nameof(SettingsPage),
            Name = "Settings Page",
            Glyph = "\uF259"
        };

        public static AppPage[] Collection => new AppPage[] 
        {
            HomePage, 
            Page1,
            Page2,
            Page3
        };

        public static AppPage? GetById(string id)
        {
            return Collection.Where(p => p.Id == id).FirstOrDefault();
        }

    }
}
