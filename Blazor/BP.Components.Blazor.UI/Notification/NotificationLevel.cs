using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Notification
{
    public enum NotificationLevel
    {
        Success,
        Warning,
        Information,
        Error
    }

    internal static class ToastLevelExtensions
    {
        internal static string GetBootstrapClass(this NotificationLevel level)
        {
            return level switch
            {
                NotificationLevel.Success => "text-bg-success",
                NotificationLevel.Warning => "text-bg-warning",
                NotificationLevel.Information => "text-bg-info",
                NotificationLevel.Error => "text-bg-danger",
                _ => throw new NotImplementedException(),
            };
        }
    }

}
