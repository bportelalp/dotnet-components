using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Toast
{
    internal class ToastEventArgs : EventArgs
    {
        internal string ToastId { get; set; }
        internal string Message { get; set; }
        internal string Title { get; set; }

    }
}
