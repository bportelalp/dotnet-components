using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Notification
{
    public interface INotificationPrompt
    {
        public void ShowConfirm(string title, string message, Action<bool> resultAction);
        public void ShowConfirm(string title, string message, string acceptButton, string cancelButton, Action<bool> resultAction);

        public void ShowPrompt(string title, string message, Action<string> resultAction);
        public void ShowPrompt(string title, string message, string acceptButton, string cancelButton, Action<string> resultAction);
    }
}
