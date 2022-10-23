using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Notification
{
    public class ToastOptions
    {
        public int Duration { get; set; } = 5000;
        public bool Stacking { get; set; } = true;
        public ToastPosition Position { get; set; } = ToastPosition.BottomRight;
    }
}
