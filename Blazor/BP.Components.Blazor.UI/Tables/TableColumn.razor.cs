using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Tables
{
    public partial class TableColumn<TRow> : ComponentBase
    {
        [Parameter] public Expression<Func<TRow, object>> Field { get; set; }
        [Parameter] public Func<TRow, object> Prefix { get; set; }
        [Parameter] public Func<TRow, object> Suffix { get; set; }
        [Parameter] public bool Sortable { get; set; } = true;
        [Parameter] public string Title { get; set; }
        [Parameter] public string Format { get; set; }
        [Parameter] public RenderFragment<TRow> ChildContent { get; set; }
        [Parameter] public bool SortFirst { get; set; }


        [CascadingParameter] public Table<TRow> OwnerTable { get; set; }




        internal event EventHandler<Tuple<Expression<Func<TRow, object>>, EOrderColumn>> OnChangeOrder;
        private Func<TRow, object> compiledField;
        private Expression lastCompiledExpression;
        private RenderFragment headerTemplate;
        private RenderFragment<TRow> cellTemplate;
        internal EOrderColumn order = EOrderColumn.None; //0-None, 1-Ascending, 2-Descending
        private bool firstSorted = false;

        // Add the column to the parent Grid component.
        // OnInitialized is called only once in the component lifecycle
        protected override void OnInitialized()
        {
            OwnerTable.AddColumn(this);
        }

        protected override void OnParametersSet()
        {
            if (lastCompiledExpression != Field)
            {
                compiledField = Field?.Compile();
                lastCompiledExpression = Field;
            }
            if (SortFirst && !firstSorted)
            {
                order = EOrderColumn.Ascending;
                firstSorted = true;
            }

        }

        internal RenderFragment HeaderTemplate
        {
            get
            {
                if (headerTemplate == null)
                {
                    headerTemplate = (builder =>
                    {
                        // Use the provided title or infer it from the expression
                        var title = Title;
                        if (title == null && Field != null)
                        {
                            title = GetMemberName(Field);
                        }

                        builder.OpenElement(0, "th");
                        builder.AddAttribute(0, "style", Sortable ? "cursor: pointer;" : "cursor: default;");
                        if (Sortable)
                            builder.AddAttribute(0, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, PrepareChangeOrder));
                        builder.AddContent(0, title);
                        if (Sortable)
                        {

                            builder.OpenElement(2, "span");
                            builder.AddAttribute(2, "class", this.GetIconOrderCSS());
                            builder.CloseElement();
                        }

                        builder.CloseElement();
                    });
                }
                return headerTemplate;
            }
        }

        /// <summary>
        /// Almacena el estado de la ordenación e invoca al padre Grid la expresión para que lo filtre
        /// </summary>
        private void PrepareChangeOrder()
        {
            if (Field is null) return;

            if (order == EOrderColumn.None)
                order = EOrderColumn.Ascending;
            else if (order == EOrderColumn.Ascending)
                order = EOrderColumn.Descending;
            else if (order == EOrderColumn.Descending)
                order = EOrderColumn.Ascending;

            OnChangeOrder?.Invoke(this, new Tuple<Expression<Func<TRow, object>>, EOrderColumn>(Field, order));
        }


        internal RenderFragment<TRow> CellTemplate
        {
            get
            {
                if (cellTemplate == null)
                {
                    cellTemplate = (rowData => builder =>
                    {
                        builder.OpenElement(0, "td");
                        if (compiledField != null)
                        {
                            var value = compiledField(rowData);
                            string formattedValue;
                            if (value?.GetType() == typeof(DateTime))
                                formattedValue = string.IsNullOrEmpty(Format) ? value?.ToString() : ((DateTime)value).ToString(Format);
                            else
                                formattedValue = string.IsNullOrEmpty(Format) ? value?.ToString() : string.Format("{0:" + Format + "}", value);

                            if (Prefix is not null)
                                formattedValue = $"{Prefix(rowData)} {formattedValue}";
                            if (Suffix is not null)
                                formattedValue = $"{formattedValue} {Suffix(rowData)}";
                            builder.AddContent(1, formattedValue);
                        }
                        else
                        {
                            builder.AddContent(2, ChildContent, rowData);
                        }

                        builder.CloseElement();
                    });
                }
                return cellTemplate;
            }
        }

        // Get the Member name from an expression.
        // (customer => customer.Name) returns "Name"
        private static string GetMemberName(Expression expression)
        {
            try
            {
                return ((MemberExpression)((LambdaExpression)expression).Body).Member.Name;
            }
            catch (Exception)
            {
                return "Title";
            }
        }

        /// <summary>
        /// Obtener el CSS del icono según orden (flecha arriba y flecha abajo o nada)
        /// </summary>
        /// <returns></returns>
        private string GetIconOrderCSS()
        {
            if (order == EOrderColumn.Ascending)
                return "oi oi-caret-top";
            if (order == EOrderColumn.Descending)
                return "oi oi-caret-bottom";
            else
                return "";
        }
    }
}
