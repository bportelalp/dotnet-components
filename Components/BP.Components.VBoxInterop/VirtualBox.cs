using BP.Components.VBoxInterop.Cmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.VBoxInterop
{
    public class VirtualBox
    {
        private CmdInterop cmd;
        private CmdInterop Cmd { get { if (cmd is null) cmd = new CmdInterop(); return cmd; } }

        public VirtualBox()
        {
        }
        public IEnumerable<VirtualMachine> GetVMs()
        {
            var output = Cmd.ListVMs();
            var lines = output.Split("\n\r").ToList();
            List<VirtualMachine> result = new List<VirtualMachine>();
            foreach (var line in lines)
            {
                var name = line.Trim().Split(" ").FirstOrDefault()?.Replace("\"","");
                var vm = new VirtualMachine(name);
                vm.CheckVMState();
                result.Add(vm);
            }
            return result;
        }
    }
}
