using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Toast
{
    public partial class Toast : ComponentBase
    {
        [Inject] IJSRuntime JS { get; set; }
        [Inject] ToastService ToastService { get; set; }


        private List<ToastEventArgs> Toasts = new List<ToastEventArgs>();

        private IJSObjectReference module;
        private DotNetObjectReference<Toast> thisReference;

        protected override void OnInitialized()
        {
            thisReference = DotNetObjectReference.Create(this);
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            ToastService.OnShowToast += OnShowToast;
            base.OnParametersSet();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JS.InvokeAsync<IJSObjectReference>("import",
                            "./_content/BP.Components.Blazor.UI/js/Toast.js");
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        private async void OnShowToast(object sender, ToastEventArgs e)
        {
            Toasts.Add(e);
            StateHasChanged();
            await Task.Delay(100);
            await module.InvokeVoidAsync("showToast", e.ToastId, thisReference);
        }




        [JSInvokable]
        public void OnHide(string toastId)
        {
            var toast = Toasts.FirstOrDefault(t => t.ToastId == toastId);
            Toasts.Remove(toast);
        }
    }
}
