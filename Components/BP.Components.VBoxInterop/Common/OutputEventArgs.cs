using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.VBoxInterop.Common
{
    public class LogEventArgs : EventArgs
    {
        public string Log { get; set; }
        public string Source { get; set; }
        public DateTime RegisteredOn { get; set; }

        public override string ToString()
        {
            return "[" + RegisteredOn.ToString() + "] - " + Log;
        }
    }
}
