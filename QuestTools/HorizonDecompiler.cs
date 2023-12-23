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
        public static void DecompileAPK(bool pullFromQuest = true)
        {
            if (pullFromQuest) PullAndExtractAPK();
            else CopyAndExtractAPK();
            ExtractDex();
            Decompile();
        }
        
        public static void DecompileAPK(string apkLocation, string packageId = "")
        {
            if (packageId != "") PublicStaticVars.settings.packageId = packageId;
            string orgResDir = PublicStaticVars.settings._resultDirectory;
            PublicStaticVars.settings._resultDirectory = PublicStaticVars.settings._resultDirectory = Directory.GetParent(apkLocation).Parent.FullName + Path.DirectorySeparatorChar;
            ExtractAPK(apkLocation);
            ExtractDex();
            Decompile();
            PublicStaticVars.settings._resultDirectory = orgResDir;
        }

        public static void CopyAndExtractAPK()
        {
            string apkL = ConsoleUiController.QuestionString("APK path: ").Replace("\"", "");
            ExtractAPK(apkL);
        }

        public static void ExtractAPK(string location)
        {
            if (!File.Exists(location)) return;
            Logger.Log("Extracting " + PublicStaticVars.settings.packageId);
            FileManager.RecreateDirectoryIfExisting(PublicStaticVars.extracedHorizonLocation);
            ZipArchive a = ZipFile.OpenRead(location);
            ZipArchiveEntry e;
            int i = 1;
            string file;
            
            while ((e = a.GetEntry(file = "classes" + (i != 1 ? i : "") + ".dex")) != null)
            {
                e.ExtractToFile(PublicStaticVars.extracedHorizonLocation + file);
                i++;
            }
            a.Dispose();
            //ZipFile.ExtractToDirectory(location, PublicStaticVars.extracedHorizonLocation);
        }

        public static void DepackApk(string location)
        {
            FileManager.RecreateDirectoryIfExisting(PublicStaticVars.extracedApkLocation);
            // extract apk
            Process.Start("java.exe", "-jar \"" + PublicStaticVars.apktoolLocation + "\" d \"" + location + "\" -f -o \"" + PublicStaticVars.extracedApkLocationWithoutSlash + "\"").WaitForExit();
        }

        public static void PullAndExtractAPK()
        {
            FileManager.CreateDirectoryIfNotExisting(PublicStaticVars.settings._resultDirectory);
            string horizonLocation = PublicStaticVars.settings.resultDirectory + "app.apk";
            Logger.Log(horizonLocation, LoggingType.Important);
            ADBHelpers.PullAPKFromQuest(horizonLocation);
            ExtractAPK(horizonLocation);
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
