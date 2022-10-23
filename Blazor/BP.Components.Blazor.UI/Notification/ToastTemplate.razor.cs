using BP.Components.Blazor.UI.Notification;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Notification
{
    public partial class ToastTemplate
    {
        [Parameter] public ToastMessage Message { get; set; }

        private string css => Message.Level.GetBootstrapClass();
        private string date => Message.CreatedOn.ToString("hh:mm:ss");
        private string promptText;


        private void OnPromptAnswered(bool result)
        {
            if (Message.Dialog.IsPrompt)
            {
                if(result)
                    Message.Dialog.ActionPrompt.Invoke(promptText);
                else
                    Message.Dialog.ActionPrompt.Invoke(null);

            }
            else
            {
                if(result)
                    Message.Dialog.ActionConfirmation.Invoke(true);
                else
                    Message.Dialog.ActionConfirmation.Invoke(false);
            }
        }
    }
}
