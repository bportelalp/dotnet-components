using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BP.Components.Blazor.UI.TreeView
{
    public partial class TreeView
    {

		#region Injects
		[Inject] public ILogger<TreeView> Logger { get; set; } = null!;
		#endregion

		#region Parameters
		[Parameter] public RenderFragment ChildContent { get; set; }
		#endregion

		#region Lifecycle
		#endregion

		#region Interface
		#endregion

		#region Methods
		#endregion

		#region UI Handlers
		#endregion

	}
}
