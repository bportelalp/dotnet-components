using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Notification
{
    public enum ToastPosition
    {
        TopLeft,
        TopRigth,
        BottomLeft,
        BottomRight,
    }

    internal static class ToastPositionExtensions
    {
        internal static string GetBootstrapClass(this ToastPosition position)
        {
            return position switch
            {
                ToastPosition.TopLeft => "top-0 start-0",
                ToastPosition.TopRigth => "top-0 end-0",
                ToastPosition.BottomLeft => "bottom-0 start-0",
                ToastPosition.BottomRight => "bottom-0 end-0",
                _ => throw new NotImplementedException(),
            };
        }
    }
}
