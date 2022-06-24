using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.FrontendUtils
{
    public class DomAccessService
    {
        private IJSObjectReference module;
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public DomAccessService(IJSRuntime js)
        {
            Js = js;
            moduleTask = new(() => js.InvokeAsync<IJSObjectReference>(
               "import", "./_content/BP.Components.Blazor.UI/js/DomAccess.js").AsTask());
        }

        public IJSRuntime Js { get; }

        public async ValueTask SetFocusElement(ElementReference reference)
        {
            module = await moduleTask.Value;
            await module.InvokeVoidAsync("setFocusElement", reference);
        }

        public async ValueTask<string> GetElementTextContent(ElementReference reference)
        {
            module = await moduleTask.Value;
            return await module.InvokeAsync<string>("getTextContent", reference);
        }
    }
}
