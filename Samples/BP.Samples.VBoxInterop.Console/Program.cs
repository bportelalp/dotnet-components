using BP.Components.VBoxInterop;
using BP.Components.VBoxInterop.Cmd;
using BP.Components.VBoxInterop.Common;

namespace BP.Samples.VBoxInterop.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var vbox = new VirtualBox();
            var vms = vbox.GetVMs();
            var webserver = vms.FirstOrDefault();
            webserver.Output += (s, e) => { System.Console.WriteLine(e.ToString()); };
            var state = webserver.GetState();
        }

    }
}