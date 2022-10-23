using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Notification
{
    internal class ToastEventArgs : EventArgs
    {
        public ToastEventArgs() { }
        public ToastEventArgs(string title, string text, NotificationLevel level, int delay)
        {
            Message = new ToastMessage()
            {
                ToastId = Guid.NewGuid().ToString(),
                Message = text,
                Title = title,
                Level = level
            };
            Delay = delay;
        }
        internal string ToastId => Message.ToastId;
        internal ToastMessage Message { get; set; }
        internal int Delay { get; set; }
    }
}
