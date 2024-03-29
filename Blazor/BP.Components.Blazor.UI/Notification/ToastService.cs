﻿using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
namespace BP.Components.Blazor.UI.Notification
{
    public class ToastService : INotification, INotificationPrompt
    {
        private readonly IStringLocalizer<Toast> locale;
        private readonly ToastOptions options;

        public ToastService(IStringLocalizer<Toast> locale, IOptions<ToastOptions> options)
        {
            if (options is null)
                this.options = new ToastOptions();
            else
                this.options = options.Value;
            this.locale = locale;
        }

        internal event EventHandler<ToastEventArgs> OnShowToast;

        public void Show(string title, string text, NotificationLevel level) => Show(title, text, level, options.Duration);
        public void Show(string title, string text, NotificationLevel level, int duration)
        {
            OnShowToast?.Invoke(this, new ToastEventArgs(title, text, level, duration));
        }

        public void ShowInformation(string text) => ShowInformation(locale["Info"], text);
        public void ShowInformation(string text, int duration) => ShowInformation(locale["Info"], text, duration);
        public void ShowInformation(string title, string text) => Show(title, text, NotificationLevel.Information);
        public void ShowInformation(string title, string text, int duration) => Show(title, text, NotificationLevel.Information, duration);

        public void ShowSuccess(string text) => ShowSuccess(locale["Success"], text);
        public void ShowSuccess(string text, int duration) => ShowSuccess(locale["Success"], text, duration);
        public void ShowSuccess(string title, string text) => Show(title, text, NotificationLevel.Success);
        public void ShowSuccess(string title, string text, int duration) => Show(title, text, NotificationLevel.Success, duration);

        public void ShowWarning(string text) => ShowWarning(locale["Warning"], text);
        public void ShowWarning(string text, int duration) => ShowWarning(locale["Warning"], text, duration);
        public void ShowWarning(string title, string text) => Show(title, text, NotificationLevel.Warning);
        public void ShowWarning(string title, string text, int duration) => Show(title, text, NotificationLevel.Warning, duration);

        public void ShowError(string text) => ShowError(locale["Error"], text);
        public void ShowError(string text, int duration) => ShowError(locale["Error"], text, duration);
        public void ShowError(string title, string text) => Show(title, text, NotificationLevel.Error);
        public void ShowError(string title, string text, int duration) => Show(title, text, NotificationLevel.Error, duration);

        public void ShowConfirm(string title, string message, Action<bool> resultAction)
            => ShowConfirm(title, message, locale["Accept"], locale["Cancel"], resultAction);
        public void ShowConfirm(string title, string message, string acceptButton, string cancelButton, Action<bool> resultAction)
        {
            ToastEventArgs args = new ToastEventArgs(title, message, NotificationLevel.Information, options.Duration);
            args.Message.Dialog = new ToastMessagePrompt(acceptButton, cancelButton, false);
            args.Message.Dialog.ActionConfirmation = resultAction;

            OnShowToast.Invoke(this, args);
        }

        public void ShowPrompt(string title, string message, Action<string> resultAction)
            => ShowPrompt(title, message, locale["Accept"], locale["Cancel"], resultAction);
        public void ShowPrompt(string title, string message, string acceptButton, string cancelButton, Action<string> resultAction)
        {
            ToastEventArgs args = new ToastEventArgs(title, message, NotificationLevel.Information, options.Duration);
            args.Message.Dialog = new ToastMessagePrompt(acceptButton, cancelButton, true);
            args.Message.Dialog.ActionPrompt = resultAction;

            OnShowToast.Invoke(this, args);
        }

        internal ToastOptions GetOptions() => options;
    }
}
