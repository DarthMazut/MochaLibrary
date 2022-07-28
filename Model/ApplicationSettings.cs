using MochaCore.Behaviours;
using MochaCore.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ApplicationSettings
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
    }
}
