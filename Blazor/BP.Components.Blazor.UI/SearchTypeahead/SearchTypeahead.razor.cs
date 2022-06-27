using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;
using BP.Components.Blazor.UI.FrontendUtils;

namespace BP.Components.Blazor.UI.SearchTypeahead
{
    public partial class SearchTypeahead<TItem> : ComponentBase, IDisposable
    {
        [Inject] DomAccessService dom { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public TItem Value { get; set; }
        [Parameter] public EventCallback<TItem> ValueChanged { get; set; }
        [Parameter] public Func<string, IEnumerable<TItem>> SearchMethod { get; set; }
        [Parameter] public RenderFragment NotFoundTemplate { get; set; }
        [Parameter] public RenderFragment<TItem> ResultTemplate { get; set; }
        [Parameter] public RenderFragment<TItem> SelectedTemplate { get; set; }
        [Parameter] public RenderFragment ResultTemplateTableHeader { get; set; }
        [Parameter] public RenderFragment<TItem> ResultTemplateTableBody { get; set; }
        [Parameter] public int MinimumLength { get; set; } = 0;
        [Parameter] public int Debounce { get; set; } = 300;
        [Parameter] public int MaximumSuggestions { get; set; } = 100;
        [Parameter] public bool DisplayClear { get; set; } = true;
        [Parameter] public string SearchText { get; set; } = string.Empty;

        protected bool IsSearching { get; private set; } = false;
        protected bool IsShowingSuggestions { get; private set; } = false;
        protected bool IsShowingSearchbar { get; private set; } = true;
        protected bool IsShowingMask { get; private set; } = false;
        protected string ResultSelected { get; private set; } = string.Empty;
        protected List<TItem> Suggestions { get; set; } = new List<TItem>();
        protected string searchText
        {
            get => _searchText;
            set
            {
                _searchText = value;

                if (value.Length == 0)
                {
                    _debounceTimer.Stop();
                    Suggestions = (SearchMethod?.Invoke(_searchText)).ToList();
                }
                else if (value.Length >= MinimumLength)
                {
                    _debounceTimer.Stop();
                    _debounceTimer.Start();
                }
            }
        }

        protected ElementReference searchInput;
        protected ElementReference typeahead;
        protected ElementReference mask;

        private System.Timers.Timer _debounceTimer;
        private string _searchText = string.Empty;

        protected override void OnInitialized()
        {
            _searchText = SearchText;
            if (SearchMethod == null)
            {
                throw new InvalidOperationException($"{GetType()} requires a {nameof(SearchMethod)} parameter.");
            }

            if (ResultTemplate == null)
            {
                if (ResultTemplateTableHeader == null || ResultTemplateTableBody == null)
                {
                    throw new InvalidOperationException($"{GetType()} requires a {nameof(ResultTemplateTableHeader)} and {nameof(ResultTemplateTableBody)} Render fragments at the same time.");
                }
                //else
                //{
                //    throw new InvalidOperationException($"{GetType()} requires a {nameof(ResultTemplate)} parameter.");
                //}

            }
            Suggestions = (SearchMethod?.Invoke(_searchText)).ToList();
            _debounceTimer = new System.Timers.Timer();
            _debounceTimer.Interval = Debounce;
            _debounceTimer.AutoReset = false;
            _debounceTimer.Elapsed += Search;

            Initialize();
        }

        protected override void OnParametersSet()
        {
            Initialize();
        }

        private void Initialize()
        {
            IsShowingSuggestions = false;
            if (Value == null)
            {
                IsShowingMask = false;
                IsShowingSearchbar = true;
            }
            else
            {
                IsShowingSearchbar = false;
                IsShowingMask = true;
            }
        }

        /// <summary>
        /// Evento desencadenado cuando se clicka sobre la máscara
        /// </summary>
        protected async void HandleClickOnMask(bool isFromButton)
        {
            Search(this, null);

            //TODO-bportela: Decidir si cambia el comportamiento o no
            if (isFromButton)
            {
                _searchText = string.Empty;
                await SelectResult(default(TItem));
            }
            else
            {
                try
                {
                    var resultado = await dom.GetElementTextContent(mask);
                    _searchText = resultado;
                }
                catch
                {
                    //No se pudo leer, paso
                    _searchText = string.Empty;
                }

            }

            IsShowingMask = false;
            IsShowingSearchbar = true;
            IsShowingSuggestions = true;
            await InvokeAsync(StateHasChanged);
            await dom.SetFocusElement(searchInput);
        }
        protected async Task ShowMaximumSuggestions()
        {
            IsShowingSuggestions = !IsShowingSuggestions;

            if (IsShowingSuggestions)
            {
                searchText = "";
                IsSearching = true;
                await InvokeAsync(StateHasChanged);


                Suggestions = (SearchMethod?.Invoke(_searchText)).ToList();

                IsSearching = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// Mostrar dropdown cuando Hay sugeencias cargadas
        /// </summary>
        protected void ShowSuggestions()
        {
            if (Suggestions.Any())
            {
                IsShowingSuggestions = true;
            }
        }


        protected async Task HandleKeyUpOnSuggestion(KeyboardEventArgs args, TItem item)
        {
            // Maybe on any key except Tab and Enter, continue the typing option.
            switch (args.Key)
            {
                case "Enter":
                    await SelectResult(item);
                    break;
                case "Escape":
                case "Backspace":
                case "Delete":
                    Initialize();
                    break;
                default:
                    break;
            }
        }

        protected string GetSelectedSuggestionClass(TItem item)
        {
            if (Value == null)
                return null;
            if (Value.Equals(item))
                return "search-typeahead-selected";
            return null;
        }

        protected async void Search(Object source, ElapsedEventArgs e)
        {
            IsSearching = true;
            await InvokeAsync(StateHasChanged);
            Suggestions = (SearchMethod?.Invoke(_searchText)).ToList();

            IsSearching = false;
            IsShowingSuggestions = true;
            await InvokeAsync(StateHasChanged);
        }

        protected async Task SelectResult(TItem item)
        {
            Value = item;
            await ValueChanged.InvokeAsync(item);
        }

        protected bool ShouldShowSuggestions()
        {
            return IsShowingSuggestions &&
                   Suggestions.Any();

        }

        private bool HasValidSearch => !string.IsNullOrWhiteSpace(searchText) && searchText.Length >= MinimumLength;

        private bool IsSearchingOrDebouncing => IsSearching || _debounceTimer.Enabled;

        protected bool ShowNotFound()
        {
            return IsShowingSuggestions &&
                   HasValidSearch &&
                   !IsSearchingOrDebouncing &&
                   !Suggestions.Any();
        }

        protected void OnFocusOut(object sender, EventArgs e)
        {
            Initialize();
            InvokeAsync(StateHasChanged);
        }

        protected void OnEscape(object sender, EventArgs e)
        {
            Initialize();
            InvokeAsync(StateHasChanged);
        }

        void IDisposable.Dispose()
        {
            _debounceTimer.Dispose();
        }
    }
}
