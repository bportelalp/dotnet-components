using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.SearchTypeahead
{
    partial class SelectorMultipleTypeahead<T> : ComponentBase
    {
        #region "Injects"
        #endregion
        #region "Cascading parameters"
        #endregion
        #region "Parameters"
        [Parameter] public List<T> Value { get; set; } = new List<T>();
        [Parameter] public EventCallback<List<T>> ValueChanged { get; set; }
        [Parameter] public Func<string, IEnumerable<T>> SearchMethod { get; set; }
        [Parameter] public RenderFragment<T> ResultTemplate { get; set; }
        [Parameter] public RenderFragment ResultTemplateTableHeader { get; set; }
        [Parameter] public RenderFragment<T> ResultTemplateTableBody { get; set; }
        [Parameter] public RenderFragment<T> ListTemplate { get; set; }
        #endregion
        #region "Privates"
        private T _itemSelected = default(T);
        private T _itemDragging;
        #endregion
        #region "Overrides"
        #endregion
        #region "Methods"
        #endregion
        #region "Events"
        #endregion
        #region "Functions"
        #endregion


        private void HandleDragStart(T item)
        {
            _itemDragging = item;
        }

        private void HandleDragOver(T item)
        {
            if (!item.Equals(_itemDragging))
            {
                var indexDraggedItem = Value.IndexOf(_itemDragging);
                var indexItem = Value.IndexOf(item);
                //Interchange
                Value[indexItem] = _itemDragging;
                Value[indexDraggedItem] = item;
            }
        }

        private void MoveDown(T item)
        {
            var indexItem = Value.IndexOf(item);
            if (indexItem == Value.Count - 1) return;
            var nextItem = Value[indexItem + 1];
            Value[indexItem] = nextItem;
            Value[indexItem + 1] = item;
        }
        private void MoveUp(T item)
        {
            var indexItem = Value.IndexOf(item);
            if (indexItem == 0) return;
            var previousItem = Value[indexItem - 1];
            Value[indexItem] = previousItem;
            Value[indexItem - 1] = item;
        }

        private void ItemSelected(T item)
        {
            if (!Value.Any(x => x.Equals(item)))
            {
                Value.Add(item);
            }
            _itemSelected = default(T);
        }
    }
}
