using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Tables
{
    public partial class Table<TRow> : ComponentBase
    {
        [Parameter] public IEnumerable<TRow> Data { get; set; }
        [Parameter] public string TableCSS { get; set; } = "table table-hover";
        [Parameter] public RenderFragment ChildContent { get; set; }
    }
}
