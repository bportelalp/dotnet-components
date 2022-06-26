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
    ///<script src = "https://cdn.jsdelivr.net/npm/sweetalert2@8" ></ script >
    /// </summary>
    public class NotificationService
    {
        private readonly IJSRuntime js;

        public NotificationService(IJSRuntime js)
        {
            this.js = js;
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
    }
}
