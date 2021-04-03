using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWPFTestApp.Settings
{
    [Serializable]
    public class GeneralSettings
    {
        public string MySetting1 { get; set; } = "Default value 1";

        public string MySetting2 { get; set; } = "Default value 2";
    }
}
