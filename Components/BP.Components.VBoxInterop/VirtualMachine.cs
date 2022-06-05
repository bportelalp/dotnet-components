using BP.Components.VBoxInterop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.VBoxInterop
{
    public class VirtualMachine
    {
        public string Name { get; set; }
        public EVMState State { get; set; }

        public VirtualMachine(string name)
        {
            Name = name;
        }

        public EVMState CheckVMState()
        {
            return State;
        }

        public Task StartVM()
        {
            var cmd = VirtualBoxCmd.CMD_START_HEADLESS(Name);
            return new Task(() => { });
        }
    }
}
