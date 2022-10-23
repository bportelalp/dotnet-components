using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI.Modal
{
    public class ModalOptions
    {
        public bool RenderBeforeOpen { get; set; } = false;
        public ModalWidth Width { get; set; } = ModalWidth.Medium;
        public ModalBackdrop Backdrop { get; set; } = ModalBackdrop.Static;
        public bool VerticallyCentered { get; set; } = false;
        public bool Scrollable { get; set; } = true;
        public bool Animated { get; set; } = true;
        public string AcceptButton { get; set; } = null;
        public string DismissButton { get; set; } = null;
    }
}
