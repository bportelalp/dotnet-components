using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Notification
{
    public interface INotification
    {
        public void Show(string title, string message, NotificationLevel level);

        public void ShowInformation(string message);
        public void ShowInformation(string title, string message);

        public void ShowSuccess(string message);
        public void ShowSuccess(string title, string message);

        public void ShowWarning(string message);
        public void ShowWarning(string title, string message);

        public void ShowError(string message);
        public void ShowError(string title, string message);
    }
}
