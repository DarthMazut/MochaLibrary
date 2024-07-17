using MochaCore.Settings;
using System.Text.Json;

namespace ModelX
{
    public class Settings : ISettingsSection
    {
        public string? MyTextSetting { get; set; }

        public async Task FillValuesAsync(string serializedData)
        {
            Settings? settings = await Task.Run(() => JsonSerializer.Deserialize<Settings>(serializedData));
            if (settings is not null)
            {
                MyTextSetting = settings.MyTextSetting;
            }
        }

        public Task<string> SerializeAsync() => Task.Run(() => JsonSerializer.Serialize(this));
    }
}
