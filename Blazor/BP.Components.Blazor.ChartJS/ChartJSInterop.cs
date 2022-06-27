using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.ChartJS
{
    public class ChartJSInterop
    {
        private readonly IJSRuntime js;
        private Lazy<Task<IJSObjectReference>> moduleTask;
        private string srcChartJS = "";
        private string srcInterop = "./_content/BP.Components.Blazor.ChartJS/js/ChartJSInterop.js";

        public ChartJSInterop(IJSRuntime js)
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
