using BP.Components.VBoxInterop.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.VBoxInterop.Cmd
{
    public class CmdInterop : IDisposable
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

        public async Task<List<string>> RunCommandAsync(string args)
        {
            var proc = this.InitializeProcess(args);
            proc.Start();
            proc.BeginOutputReadLine();
            await proc.WaitForExitAsync();
            proc.Close();
            proc.OutputDataReceived -= null;
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
            proc.Exited += new EventHandler((o, e) =>
            {

            });
            proc.OutputDataReceived += new DataReceivedEventHandler((o, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                {
                    DataReceived?.Invoke(o, e);
                    output.Add(e.Data);
                }
            });
            return proc;
        }

        public void Dispose()
        {
        }
    }
}
