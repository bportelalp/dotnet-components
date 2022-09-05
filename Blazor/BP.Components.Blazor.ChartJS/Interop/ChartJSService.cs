using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.ChartJS.Interop
{
    public class ChartJSService
    {
        private readonly IJSRuntime js;
        private Lazy<Task<IJSObjectReference>> moduleTask;
        private string srcChartJS = "./_content/BP.Components.Blazor.ChartJS/js/js-packages/chart3.8.min.js";
        private string srcInterop = "./_content/BP.Components.Blazor.ChartJS/js/ChartJSInterop.js";

        public ChartJSService(IJSRuntime js)
        {
            this.js = js;
            moduleTask = new Lazy<Task<IJSObjectReference>>(async () =>
            {
                await js.InvokeAsync<IJSObjectReference>("import", srcChartJS);
                return await js.InvokeAsync<IJSObjectReference>("import", srcInterop);
            });


        }
    }
}
