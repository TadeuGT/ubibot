using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubibot.Modulos {
    public class Jogos {
        [JsonProperty(PropertyName = "AppID ")]
        public string AppID;
        public string MDM;
        public string SpaceID;
        public string Category;
        public string Name;
        public string Platform;
        public string Status;
        public string Maintenance;
        public string[] ImpactedFeatures;

    }
}
