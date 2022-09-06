using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.VBoxInterop.Common
{
    public enum EVMState
    {
        Unknown = 0,
        Stopped = 1,
        Starting = 2,
        Running = 3,
        Pausing = 4,
        Paused = 5,
    }
}
