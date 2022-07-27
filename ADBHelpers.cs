using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestTools
{
    public class ADBHelpers
    {
        public static void PullAPKFromQuest(string target, string packageID = "")
        {
            if (packageID == "") packageID = PublicStaticVars.settings.packageId;
            string location = PublicStaticVars.interactor.adbS("shell pm path " + packageID).Replace("package:", "").Replace("\r\n", "");
            PublicStaticVars.interactor.Pull(location, target);
        }
    }
}
