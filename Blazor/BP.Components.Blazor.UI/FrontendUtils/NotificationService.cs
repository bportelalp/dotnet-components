using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.FrontendUtils
{
    /// <summary>
    /// Incluir librería 
    ///<script src = "https://cdn.jsdelivr.net/npm/sweetalert2@11.4.18/dist/sweetalert2.all.min.js" ></ script >
    ///
    /// </summary>
    public class NotificationService
    {
        private readonly IJSRuntime js;

        private IJSObjectReference module;
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public NotificationService(IJSRuntime js)
        {
            this.js = js;
            moduleTask = new(() =>
                js.InvokeAsync<IJSObjectReference>("import", "./_content/BP.Components.Blazor.UI/js/Notifications.js").AsTask());
        }

        public async Task ShowError(string mensaje)
        {
            await ShowMessage("Error", mensaje, "error");
        }

        public async Task ShowSucess(string mensaje)
        {
            await ShowMessage("Éxito", mensaje, "success");
        }

        private async ValueTask ShowMessage(string titulo, string mensaje, string tipoMensaje)
        {
            await js.InvokeVoidAsync("Swal.fire", titulo, mensaje, tipoMensaje);
        }

        public async ValueTask<bool> ShowPrompt(string title, string acceptButton = "Ok", string denyButton = "Cancel")
        {
            module = await moduleTask.Value;
            return await module.InvokeAsync<bool>("swalPrompt", title, acceptButton, denyButton);
        }
    }
}
