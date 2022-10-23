using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Notification
{
    public interface INotificationPrompt
    {
        public void ShowConfirm(string title, string message, string acceptText, string denyText, Action<bool> resultAction);
        public Task<string> ShowPrompt(string title, string message, string acceptText, string denyText, Action<bool> resultAction);
    }
}
