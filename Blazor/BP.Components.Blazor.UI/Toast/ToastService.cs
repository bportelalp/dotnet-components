using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Toast
{
    public class ToastService
    {
        private readonly IJSRuntime js;
        private Task<IJSObjectReference> moduleTask;

        public ToastService(IJSRuntime js)
        {
            this.js = js;
            
        }

        internal event EventHandler<ToastEventArgs> OnShowToast;


        public async ValueTask ShowToast()
        {
            OnShowToast.Invoke(this, new ToastEventArgs()
            {
                Title = "fafawdfw",
                Message = $"Mensaje {Guid.NewGuid()}",
                ToastId = Guid.NewGuid().ToString()
            });
            
        }


    }
}
