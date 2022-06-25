using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Tables
{
    public partial class TableCollapsibleRow<TRow>
    {
        [Parameter] public RenderFragment<TRow> ChildContent { get; set; }

        [CascadingParameter] public Table<TRow> OwnerTable { get; set; }
        protected override void OnInitialized()
        {
            OwnerTable.AddCollapsibleRow(this);
        }
        internal RenderFragment<TRow> row;
        internal RenderFragment<TRow> Row
        {
            get
            {
                if(row is null)
                {
                    row = (rowData => builder =>
                    {
                        if(rowData is null)
                        {
                            row = null;
                            return;
                        }
                        builder.OpenElement(0, "tr");
                        builder.OpenElement(1, "td");
                        builder.AddAttribute(1, "colspan", "100%");
                        builder.AddContent(1, ChildContent, rowData);
                        builder.CloseComponent();
                        builder.CloseComponent();
                    });
                }
                return row;
            }
        }
    }
}
