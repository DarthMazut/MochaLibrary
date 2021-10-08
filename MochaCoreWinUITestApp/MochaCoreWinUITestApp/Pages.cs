using Microsoft.UI.Xaml.Controls;
using MochaCore.Navigation;

namespace MochaCoreWinUITestApp
{
    public static class Pages
    {
        public static PageInfo Page1 { get; } = new PageInfo("Page1")
        {
            Name = "Page 1",
            FontIcon = "\xE10F"
        };

        public static PageInfo Page2 { get; } = new PageInfo("Page2")
        {
            Name = "Page 2",
            FontIcon = "\xE702"
        };
    }
}
