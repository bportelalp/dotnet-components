using BP.Components.Blazor.UI.Notification;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Modal
{
    public partial class ModalWindow
    {
        [Inject] IJSRuntime JS { get; set; }
        [Parameter] public RenderFragment ModalHeader { get; set; }
        [Parameter] public RenderFragment ModalBody { get; set; }
        [Parameter] public RenderFragment ModalFooter { get; set; }
        [Parameter] public ModalOptions Options { get; set; } = new ModalOptions();
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public EventCallback OnAccept { get; set; }

        private readonly string ModalId = Guid.NewGuid().ToString();
        private string animatedCss => Options.Animated ? "fade" : "";
        private string scrollableCss => Options.Scrollable ? "modal-dialog-scrollable" : "";
        private string centeredCss => Options.VerticallyCentered ? "modal-dialog-centered" : "";
        private string sizeCss => Options.Width.GetBootstrapCss();

        private IJSObjectReference module;

        private bool rendered = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JS.InvokeAsync<IJSObjectReference>("import",
                            "./_content/BP.Components.Blazor.UI/js/Modal.js");
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public async void OpenModal()
        {
            await HandleRendering(true);
            _ = module.InvokeVoidAsync("showModal", ModalId);
        }

        public async void CloseModal()
        {
            await HandleRendering(false);
            _ = module.InvokeVoidAsync("hideModal", ModalId);
        }

        public async Task HandleRendering(bool render)
        {
            if (!Options.RenderBeforeOpen && rendered != render)
            {
                rendered = render;
                StateHasChanged();
                await Task.Delay(300);
            }
        }
    }
}
