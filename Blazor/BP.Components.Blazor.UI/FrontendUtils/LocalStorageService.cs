using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.FrontendUtils
{
    public class LocalStorageService
    {
        private IJSObjectReference module;
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public LocalStorageService(IJSRuntime js)
        {
            Js = js;
            moduleTask = new(() => js.InvokeAsync<IJSObjectReference>(
               "import", "./_content/BP.Components.Blazor.UI/js/SessionManager.js").AsTask());
            //module = js.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorComponents.UI/js/SessionManager.js").Result;
        }

        public IJSRuntime Js { get; }

        public async ValueTask SetItem<T>(string key, T value)
        {
            module = await moduleTask.Value;
            await module.InvokeVoidAsync("setItem", key,value);
        }

        public async ValueTask<T> GetItem<T>(string key)
        {
            module = await moduleTask.Value;
            return await module.InvokeAsync<T>("getItem", key);
        }

        public async ValueTask DeleteItem(string key)
        {
            module = await moduleTask.Value;
            await module.InvokeVoidAsync("deleteItem", key);
        }

    }
}
