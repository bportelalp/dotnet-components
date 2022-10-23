using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Notification
{
    public class SweetAlertService : INotification, INotificationPrompt
    {
        private readonly IJSRuntime js;
        private readonly IStringLocalizer<Toast> locale;
        private IJSObjectReference module;
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public SweetAlertService(IJSRuntime js, IStringLocalizer<Toast> locale)
        {
            this.js = js;
            this.locale = locale;
            moduleTask = new(() =>
                js.InvokeAsync<IJSObjectReference>("import", "./_content/BP.Components.Blazor.UI/js/Notifications.js").AsTask());
        }

        public void Show(string title, string message, NotificationLevel level)
        {
            js.InvokeVoidAsync("Swal.fire", title, message, GetmessageType(level));
        }

        public void ShowInformation(string message) => Show(locale["Info"], message, NotificationLevel.Information);
        public void ShowInformation(string title, string message) => Show(title, message, NotificationLevel.Information);

        public void ShowSuccess(string message) => Show(locale["Success"], message, NotificationLevel.Success);
        public void ShowSuccess(string title, string message) => Show(title, message, NotificationLevel.Success);

        public void ShowWarning(string message) => Show(locale["Warning"], message, NotificationLevel.Warning);
        public void ShowWarning(string title, string message) => Show(title, message, NotificationLevel.Warning);

        public void ShowError(string message) => Show(locale["Error"], message, NotificationLevel.Error);
        public void ShowError(string title, string message) => Show(title, message, NotificationLevel.Error);

        public void ShowConfirm(string title, string message, Action<bool> resultAction) 
            => ShowConfirm(title, message, locale["Accept"], locale["Cancel"], resultAction);
        public async void ShowConfirm(string title, string message, string acceptButton, string cancelButton, Action<bool> resultAction)
        {
            module = await moduleTask.Value;
            var response = await module.InvokeAsync<bool>("swalPrompt", title, acceptButton, cancelButton);
            resultAction.Invoke(response);
        }

        public void ShowPrompt(string title, string message, Action<string> resultAction) 
            => ShowPrompt(title, message, locale["Accept"], locale["Cancel"], resultAction);
        public void ShowPrompt(string title, string message, string acceptText, string cancelText, Action<string> resultAction)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<bool> ShowPrompt(string title, string acceptButton, string cancelButton)
        {
            module = await moduleTask.Value;
            return await module.InvokeAsync<bool>("swalPrompt", title, acceptButton, cancelButton);
        }

        private string GetmessageType(NotificationLevel level)
        {
            return level switch
            {
                NotificationLevel.Success => "success",
                NotificationLevel.Warning => "warning",
                NotificationLevel.Information => "info",
                NotificationLevel.Error => "error",
                _ => throw new NotImplementedException(),
            };
        }

        

        
    }
}
