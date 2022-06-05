using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.VBoxInterop.Common
{
    public struct VirtualBoxCmd
    {
        public static readonly string VBOXMANAGER = "VBoxManage";
        public static readonly string CMD_START_VM = "startvm";
        public static readonly string CMD_LIST_VMS = "list vms";
        public static readonly string OPT_HEADLESS = "--type headless";
        public static readonly string CMD_CTRL_VM = "controlvm";
        public static readonly string OPT_CTRL_SAVE = "savestate";
        public static readonly string OPT_CTRL_RESUME = "resume";
        public static readonly string OPT_CTRL_RESET = "reset";
        public static readonly string OPT_CTRL_POWEROFF_FORCE = "poweroff";
        public static readonly string OPT_CTRL_POWEROFF = "acpipowerbutton";

        public static string CMD_START_HEADLESS(string vmName) => CMD_START_VM + " " + vmName + " " + OPT_HEADLESS;
        public static string CMD_SAVE_STATE(string vmName) => CMD_CTRL_VM + " " + vmName + " " + OPT_CTRL_SAVE;
        public static string CMD_POWEROFF(string vmName, bool force = false) => CMD_CTRL_VM + " " + vmName + " " + (force? OPT_CTRL_POWEROFF_FORCE:OPT_CTRL_POWEROFF);
    }
}
