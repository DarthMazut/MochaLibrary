using MochaCore.Behaviours;
using MochaCore.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ApplicationSettings : ISettingsSection
    {
        public ApplicationSettings()
        {
            ImagesPath = BehaviourManager.Recall<object, string>("GetLocalAppFolderPath").Execute(new object());
        }

        public string ImagesPath { get; set; }

        public List<Person> People { get; set; } = new();

        public static string SettingsName => "appSettings";

        public static ISettingsSectionProvider<ApplicationSettings> Section => SettingsManager.Retrieve<ApplicationSettings>(SettingsName);

        public static string SettingsFolder => BehaviourManager.Recall<object, string>("GetLocalAppFolderPath").Execute(new object ());

        public static string SettingsPath => Path.Combine(SettingsFolder, SettingsName);

        public Task<string> SerializeAsync()
        {
            return Task.Run(() => JsonConvert.SerializeObject(this));
        }

        public async Task FillValuesAsync(string serializedData)
        {
            ApplicationSettings? deserializedSettings = await Task.Run(() => JsonConvert.DeserializeObject<ApplicationSettings>(serializedData));
            if (deserializedSettings is not null)
            {
                this.ImagesPath = deserializedSettings.ImagesPath;
                this.People = deserializedSettings.People;
            }
        }
    }
}
