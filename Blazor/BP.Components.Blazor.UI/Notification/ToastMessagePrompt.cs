using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Notification
{
    public class ToastMessagePrompt
    {
        public ToastMessagePrompt(string acceptButton, string denyButton, bool isPrompt)
        {
            AcceptButton = acceptButton;
            DenyButton = denyButton;
            IsPrompt = isPrompt;
        }
        public string AcceptButton { get; set; }
        public string DenyButton { get; set; }
        public bool IsPrompt { get; set; }
        public Action<bool> ActionConfirmation { get; set; }
        public Action<string> ActionPrompt { get; set; }
    }
}
