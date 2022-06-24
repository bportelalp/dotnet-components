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
        [CascadingParameter] public Table<TRow> OwnerTable { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public Func<TRow,object> Field { get; set; }
        [Parameter] public string Format { get; set; }

        [CascadingParameter] public Table<TRow> OwnerGrid { get; set; }
        [Parameter] public Expression<Func<TRow, object>> Expression { get; set; }
        [Parameter] public Expression<Func<TRow, object>> Prefix { get; set; }
        [Parameter] public Expression<Func<TRow, object>> Suffix { get; set; }
        [Parameter] public RenderFragment<TRow> ChildContent { get; set; }
        [Parameter] public bool Sortable { get; set; } = true;

        public event EventHandler<Tuple<Expression<Func<TRow, object>>, bool>> OnChangeOrder;
        private Func<TRow, object> compiledExpression;
        private Expression lastCompiledExpression;
        private RenderFragment headerTemplate;
        private RenderFragment<TRow> cellTemplate;
        public int order = 0; //0-None, 1-Ascending, 2-Descending

        private Func<TRow, object> compiledExpressionPrefix;
        private Expression lastCompiledExpressionPrefix;
        private Func<TRow, object> compiledExpressionSuffix;
        private Expression lastCompiledExpressionSuffix;
        // Add the column to the parent Grid component.
        // OnInitialized is called only once in the component lifecycle
        protected override void OnInitialized()
        {
            OwnerGrid.AddColumn(this);
        }

        protected override void OnParametersSet()
        {
            if (lastCompiledExpression != Expression)
            {
                compiledExpression = Expression?.Compile();
                lastCompiledExpression = Expression;
            }
            if (lastCompiledExpressionPrefix != Prefix)
            {
                compiledExpressionPrefix = Prefix?.Compile();
                lastCompiledExpressionPrefix = Prefix;
            }
            if (lastCompiledExpressionSuffix != Suffix)
            {
                compiledExpressionSuffix = Suffix?.Compile();
                lastCompiledExpressionSuffix = Suffix;
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
                        if (title == null && Expression != null)
                        {
                            title = GetMemberName(Expression);
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
            if (Expression is null) return;

            bool ordering = true;
            if (order == 0)
                order = 1;
            else if (order == 1)
            {
                order = 2;
                ordering = false;
            }
            else if (order == 2)
                order = 1;

            OnChangeOrder?.Invoke(this, new Tuple<Expression<Func<TRow, object>>, bool>(Expression, ordering));
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
                        if (compiledExpression != null)
                        {
                            var value = compiledExpression(rowData);
                            string formattedValue;
                            if (value?.GetType() == typeof(DateTime))
                                formattedValue = string.IsNullOrEmpty(Format) ? value?.ToString() : ((DateTime)value).ToString(Format);
                            else
                                formattedValue = string.IsNullOrEmpty(Format) ? value?.ToString() : string.Format("{0:" + Format + "}", value);

                            if (compiledExpressionPrefix is not null)
                                formattedValue = $"{compiledExpressionPrefix(rowData)} {formattedValue}";
                            if (compiledExpressionSuffix is not null)
                                formattedValue = $"{formattedValue} {compiledExpressionSuffix(rowData)}";
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
            if (order == 1)
                return "oi oi-caret-top";
            if (order == 2)
                return "oi oi-caret-bottom";
            else
                return "";
        }
    }
}
