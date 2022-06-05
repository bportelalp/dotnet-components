using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Services
{
    public class LocalStorageService
    {
        private IJSObjectReference module;
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public LocalStorageService(IJSRuntime js)
        {
            Js = js;
            moduleTask = new(() => js.InvokeAsync<IJSObjectReference>(
               "import", "./_content/BlazorComponents.UI/ts/SessionManager.js").AsTask());
            //module = js.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorComponents.UI/js/SessionManager.js").Result;
        }

        public IJSRuntime Js { get; }

        public async ValueTask SetItem(string key, string value)
        {
            module = await moduleTask.Value;
            await module.InvokeVoidAsync("setItem", key,value);
        }

        public async ValueTask<string> GetItem(string key)
        {
            module = await moduleTask.Value;
            return await module.InvokeAsync<string>("getItem", key);
        }

    }
}
