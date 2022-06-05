using BP.Components.VBoxInterop;
using BP.Components.VBoxInterop.Cmd;

namespace BP.Test.VBoxInterop.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var vbox = new VirtualBox();
            vbox.GetVMs();
        }
    }
}