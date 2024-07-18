using MochaCore.Settings;
using System.Text.Json;

namespace ModelX
{
    public class Settings : ISettingsSection
    {
        public bool Switch1 { get; set; }

        public SettingsOptionType OptionType { get; set; }

        public string? Text { get; set; }

        public async Task FillValuesAsync(string serializedData)
        {
            Settings? settings = await Task.Run(() => JsonSerializer.Deserialize<Settings>(serializedData));
            if (settings is not null)
            {
                Switch1 = settings.Switch1;
                OptionType = settings.OptionType;
                Text = settings.Text;
            }
        }

        public Task<string> SerializeAsync() => Task.Run(() => JsonSerializer.Serialize(this));
    }

    public enum SettingsOptionType
    {
        Option1Enum,
        Option2Enum,
        Option3Enum,
        Option4Enum
    }
}
