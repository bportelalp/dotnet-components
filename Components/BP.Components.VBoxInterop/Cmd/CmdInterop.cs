using BP.Components.VBoxInterop.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.VBoxInterop.Cmd
{
    public class CmdInterop
    {
        public string ListVMs()
        {
            var proc = this.InitializeProcess(VirtualBoxCmd.CMD_LIST_VMS);
            proc.Start();
            var result = proc.StandardOutput.ReadToEnd();
            return result;
        }

        public string GetInfo()
        {
            var proc = this.InitializeProcess("showvminfo \"webserver\" --machinereadable");
            proc.Start();
            var result = proc.StandardOutput.ReadToEnd();
            return result;
        }

        private Process InitializeProcess(string arguments)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = VirtualBoxCmd.VBOXMANAGER;
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            return proc;
        }
    }
}
