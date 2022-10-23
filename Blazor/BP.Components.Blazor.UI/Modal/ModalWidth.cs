using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Modal
{
    public enum ModalWidth
    {
        Small,
        Medium,
        Large,
        ExtraLarge
    }

    internal static class ModalWidthExtensions
    {
        internal static string GetBootstrapCss(this ModalWidth modalWidth)
        {
            return modalWidth switch
            {
                ModalWidth.Small => "modal-sm",
                ModalWidth.Medium => string.Empty,
                ModalWidth.Large => "modal-lg",
                ModalWidth.ExtraLarge => "modal-xl",
                _ => throw new NotImplementedException(),
            };
        }
    }
}
