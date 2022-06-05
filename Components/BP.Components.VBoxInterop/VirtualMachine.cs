using BP.Components.VBoxInterop.Cmd;
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

        private CmdInterop cmd;
        private CmdInterop Cmd { get { if (cmd is null) cmd = new CmdInterop(); return cmd; } }

        public EVMState CheckVMState()
        {
            var result = Cmd.GetInfo();
            Dictionary<string, string> info = new Dictionary<string, string>();
            var lines = result.Split("\r\n");
            foreach (var line in lines)
            {
                if (line.Contains("="))
                {
                    var pair = line.Replace("\"", "").Split("=");
                    if (!info.ContainsKey(pair[0]))
                        info.Add(pair[0], pair[1]);
                }

            }
            return State;
        }

        public Task StartVM()
        {
            var cmd = VirtualBoxCmd.CMD_START_HEADLESS(Name);
            return new Task(() => { });
        }
    }
}
