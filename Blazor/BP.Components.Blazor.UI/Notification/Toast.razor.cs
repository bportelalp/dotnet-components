using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Notification
{
    public partial class Toast : ComponentBase, IDisposable
    {
        [Inject] IJSRuntime JS { get; set; }
        [Inject] ToastService ToastService { get; set; }



        private IJSObjectReference module;
        private DotNetObjectReference<Toast> thisReference;
        private ToastOptions options;

        private readonly List<ToastMessage> toasts = new List<ToastMessage>();

        protected override void OnInitialized()
        {
            thisReference = DotNetObjectReference.Create(this);
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            ToastService.OnShowToast += OnShowToast;
            options = ToastService.GetOptions();
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
            toasts.Add(e.Message);
            StateHasChanged();
            await Task.Delay(100);

            var opt = new
            {
                delay = e.Delay
            };
            await module.InvokeVoidAsync("showToast", e.ToastId, thisReference, opt);
        }

        [JSInvokable]
        public void OnHide(string toastId)
        {
            var toast = toasts.FirstOrDefault(t => t.ToastId == toastId);
            toasts.Remove(toast);
        }

        public void Dispose()
        {
            ToastService.OnShowToast -= OnShowToast;
        }
    }
}
