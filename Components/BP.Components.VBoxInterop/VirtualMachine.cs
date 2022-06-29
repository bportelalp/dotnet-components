using BP.Components.VBoxInterop.Cmd;
using BP.Components.VBoxInterop.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.VBoxInterop
{
    public class VirtualMachine
    {
        public string Name { get; set; }
        public EVMState State { get; set; }
        public Dictionary<string, string> MachineInfo { get; set; }
        public event EventHandler<LogEventArgs> Output;

        public VirtualMachine(string name)
        {
            Name = name;
        }

        private CmdInterop cmd;
        private CmdInterop Cmd { get { if (cmd is null) { cmd = new CmdInterop(); cmd.DataReceived += NotifyNewLogEvent; } return cmd; } }

        private async Task GetMachineInfo()
        {
            var lines = await Cmd.RunCommandAsync(VirtualBoxCmd.CMD_INFO_VM(this.Name));
            MachineInfo = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line.Contains("="))
                {
                    var pair = line.Replace("\"", "").Split("=");
                    if (!MachineInfo.ContainsKey(pair[0]))
                        MachineInfo.Add(pair[0], pair[1]);
                }

            }
        }

        public async Task StartVM()
        {
            await Cmd.RunCommandAsync(VirtualBoxCmd.CMD_START_HEADLESS(Name));
        }

        public async Task SaveVM()
        {
            await Cmd.RunCommandAsync(VirtualBoxCmd.CMD_SAVE_STATE(Name));
        }

        public async Task<string> GetState()
        {
            await this.GetMachineInfo();
            return MachineInfo["VMState"];
        }

        public void NotifyNewLogEvent(object sender, DataReceivedEventArgs e)
        {
            Output?.Invoke(this, new LogEventArgs()
            {
                Log = e?.Data,
                RegisteredOn = DateTime.Now
            });
        }
    }
}
