using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Notification
{
    public class ToastMessage
    {
        public string ToastId { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public NotificationLevel Level { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public ToastMessagePrompt Dialog { get; set; } = null;

    }
}
