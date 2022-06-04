using ComputerUtils.ADB;
using ComputerUtils.ConsoleUi;
using ComputerUtils.FileManaging;
using ComputerUtils.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestTools
{
    public class HorizonDecompiler
    {
        public static List<string> jars = new List<string>();
        public static void DecompileAPK()
        {
            PullAndExtractAPK();
            ExtractDex();
            Decompile();
        }

        public static void PullAndExtractAPK()
        {
            string location = PublicStaticVars.interactor.adbS("shell pm path " + PublicStaticVars.settings.packageId).Replace("package:", "").Replace("\r\n", "");
            string horizonLocation = PublicStaticVars.settings.resultDirectory + "app.apk";
            PublicStaticVars.interactor.Pull(location, horizonLocation);
            Logger.Log("Extracting Horizon");
            FileManager.DeleteDirectoryIfExisting(PublicStaticVars.extracedHorizonLocation);
            ZipFile.ExtractToDirectory(horizonLocation, PublicStaticVars.extracedHorizonLocation);
        }

        public async static void ExtractDex()
        {
            Logger.Log("I like D-Rexes");
            int i = 1;
            jars = new List<string>();
            string file;
            FileManager.RecreateDirectoryIfExisting(PublicStaticVars.horizonJarLocation);
            while(File.Exists(file = PublicStaticVars.extracedHorizonLocation + "classes" + (i != 1 ? i : "") + ".dex"))
            {
                Logger.Log("Converting " + file + " to jar");
                jars.Add("\"" + PublicStaticVars.horizonJarLocation + Path.GetFileNameWithoutExtension(file) + ".jar\"");
                // YOU STUPID THING IT FUCKING OUTPUT
                Logger.Log("IT FUCKING OUTPUTS MAKE IT STOP");
                Process p = Process.Start("\"" + PublicStaticVars.dexToolsLocation + "d2j-dex2jar.bat\"" + " --force -o \"" + PublicStaticVars.horizonJarLocation + Path.GetFileNameWithoutExtension(file) + ".jar\" \"" + file + "\"");
                p.WaitForExit();
                i++;
            }
        }

        public static void Decompile()
        {
            FileManager.RecreateDirectoryIfExisting(PublicStaticVars.horizonSourceLocation);
            foreach (string f in jars)
            {
                Logger.Log("Decompiling " + f);
                Process p = Process.Start("\"" + PublicStaticVars.jdCliLocation + "jd-cli.bat\" \"" + f + "\" -od \"" + PublicStaticVars.horizonSourceLocation.TrimEnd(Path.DirectorySeparatorChar) + "\"");
                p.WaitForExit(10000);
                if(!p.HasExited) p.Kill();
            }
        }
    }
}
