using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Toast
{
    public partial class ToastTemplate
    {
        [Parameter] public ToastMessage Message { get; set; }
        [Parameter] public int ToastType { get; set; }
    }
}
