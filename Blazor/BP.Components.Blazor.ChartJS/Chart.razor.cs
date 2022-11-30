using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.ChartJS
{
    public partial class Chart
    {
        [Inject] protected Interop.ChartJSService Service { get; set; }

        [Parameter] public Configuration.ConfigBase Config { get; set; }
    }
}
