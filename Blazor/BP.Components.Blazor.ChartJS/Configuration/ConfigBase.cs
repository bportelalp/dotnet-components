using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BP.Components.Blazor.ChartJS.Configuration;
using Newtonsoft.Json;

namespace BP.Components.Blazor.ChartJS.Configuration
{
    public class ConfigBase
    {
        [JsonIgnore]
        public string CanvasId { get; } = Guid.NewGuid().ToString();

        public ChartType Type { get; set; }

        public ChartData Data { get; set; }

        public OptionsBase Options { get; set; }
    }
}
