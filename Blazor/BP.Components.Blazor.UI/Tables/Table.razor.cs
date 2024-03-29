﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Tables
{
    public partial class Table<TRow> : ComponentBase, IDisposable
    {
        [Parameter] public IEnumerable<TRow> Items { get; set; }
        [Parameter] public string TableCSS { get; set; } = "table table-hover";
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> TableAttributes { get; set; }

        [Parameter] public EventCallback<int> onShowMoreClick { get; set; }
        [Parameter] public EventCallback<TRow> OnRowClick { get; set; }
        [Parameter] public string TableClass { get; set; } = "table table-striped";
        [Parameter] public string HeaderClass { get; set; } = "thead-dark";
        /// <summary>
        /// Parametro de optimización. determina si las columnas son dinámicas en sucesivas renderizaciones para tener que regenerarlas
        /// </summary>
        [Parameter] public bool FixedContent { get; set; } = true;
        [Parameter] public int PageSize { get; set; } = 25;
        [Parameter] public Func<TRow, int, string> RowClass { get; set; }
        [Parameter] public bool BackToInitialPageOnItemsChange { get; set; } = false;

        public int Page { get; set; } = 1;
        protected int itemsCount = 0;



        protected readonly List<TableColumn<TRow>> Columns = new List<TableColumn<TRow>>();
        protected TableCollapsibleRow<TRow> RowCollapsible = null;
        protected List<int> ExpandedRows = new List<int>();

        // GridColumn uses this method to add a column
        internal void AddColumn(TableColumn<TRow> column)
        {
            Columns.Add(column);
            if (column.Sortable)
                column.OnChangeOrder += OrderRequested;
        }

        internal void AddCollapsibleRow(TableCollapsibleRow<TRow> row)
        {
            RowCollapsible = row;
        }

        internal void ExpandRow(int itemHash)
        {
            if(ExpandedRows.Contains(itemHash))
                ExpandedRows.Remove(itemHash);
            else
                ExpandedRows.Add(itemHash);
            StateHasChanged();
        }

        internal string ExpandRowCSS(int itemHash)
        {
            if (ExpandedRows.Contains(itemHash)) return "oi oi-minus";
            else return "oi oi-plus";
        }

        /// <summary>
        /// Método invocado cuando se produce una solicitud de orden desde los hijos. Se resetean las 
        /// ordenaciones de columnas no implicadas y se aplica a la lista de Items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OrderRequested(object sender, Tuple<Expression<Func<TRow, object>>, EOrderColumn> e)
        {
            if (e == null) return;
            foreach (var column in Columns.Where(c => c != sender))
                column.order = EOrderColumn.None;

            // Aplica la expresión a la lista de objetos
            var compiled = e.Item1.Compile();
            if (e.Item2 == EOrderColumn.Ascending)
                Items = Items.OrderBy(c => compiled(c)).ToList();
            else if (e.Item2 == EOrderColumn.Descending)
                Items = Items.OrderByDescending(c => compiled(c)).ToList();


            StateHasChanged();

        }

        protected override void OnParametersSet()
        {
            //Reorder according column if exist, (produced by a button)
            if (Columns.Count != 0)
            {
                foreach (var column in Columns)
                {
                    if (column.order != EOrderColumn.None)
                    {
                        var compiled = column.Field.Compile();
                        if (column.order == EOrderColumn.Ascending)
                            Items = Items.OrderBy(c => compiled(c)).ToList();
                        else
                            Items = Items.OrderByDescending(c => compiled(c)).ToList();
                        break;
                    }
                }
            }
            base.OnParametersSet();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            if (BackToInitialPageOnItemsChange && itemsCount != Items.Count())
            {
                Page = 1;
            }
            itemsCount = Items.Count();
            if (firstRender)
            {
                foreach (var column in Columns)
                {
                    if (column.order != EOrderColumn.None)
                    {
                        var compiled = column.Field.Compile();
                        if (column.order == EOrderColumn.Ascending)
                            Items = Items.OrderBy(c => compiled(c)).ToList();
                        else
                            Items = Items.OrderByDescending(c => compiled(c)).ToList();
                        break;
                    }
                }
                StateHasChanged();
            }
        }

        protected async Task ShowMore(int increment)
        {
            Page = Page + increment;
            if (Page < 1)
            {
                Page = 0;
            }
            if (Page > Page * PageSize)
            {
                Page = Page * PageSize;
            }
            int newTop = (Page * PageSize) + 1;
            await onShowMoreClick.InvokeAsync(newTop);
        }
        protected async Task ShowPage(int pageNumber)
        {
            Page = pageNumber;
            int newTop = (Page * PageSize) + 1;
            await onShowMoreClick.InvokeAsync(newTop);
        }

        /// <summary>
        /// Desvincula los eventos lanzados por los hijos del método
        /// </summary>
        void IDisposable.Dispose()
        {
            foreach (var col in Columns)
            {
                col.OnChangeOrder -= OrderRequested;
            }
        }
    }
}
