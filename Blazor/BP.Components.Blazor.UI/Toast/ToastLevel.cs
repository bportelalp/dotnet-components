using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Toast
{
    public enum ToastLevel
    {
        Success,
        Warning,
        Information,
        Error
    }

    internal static class ToastLevelExtensions
    {
        internal static string GetBootstrapClass(this ToastLevel level)
        {
            return level switch
            {
                ToastLevel.Success => "text-bg-success",
                ToastLevel.Warning => "text-bg-warning",
                ToastLevel.Information => "text-bg-info",
                ToastLevel.Error => "text-bg-danger",
                _ => throw new NotImplementedException(),
            };
        }
    }

}
