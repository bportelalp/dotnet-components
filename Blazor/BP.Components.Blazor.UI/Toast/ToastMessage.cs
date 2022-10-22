using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Toast
{
    public class ToastMessage
    {
        public string ToastId { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public ToastLevel Level { get; set; }

    }
}
