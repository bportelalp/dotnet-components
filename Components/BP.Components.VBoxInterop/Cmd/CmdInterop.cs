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
        public event DataReceivedEventHandler DataReceived;
        private List<string> output;
        public string ListVMs()
        {
            var proc = this.InitializeProcess(VirtualBoxCmd.CMD_LIST_VMS);
            proc.Start();
            var result = proc.StandardOutput.ReadToEnd();
            return result;
        }

        public List<string> RunCommand(string args)
        {
            var proc = this.InitializeProcess(args);
            proc.Start();
            proc.BeginOutputReadLine();
            proc.WaitForExit();
            //var result = proc.StandardOutput.ReadToEnd();
            proc.Close();
            var result = output;
            return result;
        }

        private Process InitializeProcess(string arguments)
        {
            output = new List<string>();
            Process proc = new Process();
            proc.StartInfo.FileName = VirtualBoxCmd.VBOXMANAGER;
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.OutputDataReceived += new DataReceivedEventHandler((o,e) =>
            {
                DataReceived?.Invoke(o, e);
                if(!string.IsNullOrWhiteSpace(e.Data))
                    output.Add(e.Data);
            });
            return proc;
        }
    }
}
