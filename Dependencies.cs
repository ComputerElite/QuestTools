using ComputerUtils.ConsoleUi;
using ComputerUtils.FileManaging;
using ComputerUtils.Logging;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestTools
{
    public class Dependencies
    {
        public const string dexToolsVersion = "2.1";
        public const string dexToolsDownloadLink = "https://github.com/pxb1988/dex2jar/releases/download/v2.1/dex2jar-2.1.zip";
        public const string jdCliVersion = "1.6.6";
        public const string jdCliDownloadLink = "https://github.com/intoolswetrust/jd-cli/releases/download/jd-cli-1.2.0/jd-cli-1.2.0-dist.zip";
        public const string il2CppDumperVersion = "6.7.19";
        public const string il2CppDumperDownloadLink = "https://github.com/Perfare/Il2CppDumper/releases/download/v6.7.19/Il2CppDumper-net6-v6.7.19.zip";
        public static void DownloadAndValidateDependencies()
        {
            Logger.Log("Downloading and validating dependencies");
            ValidateDexTools();
            ValidateJdCli();
            ValidateIl2CppDumper();
        }

        public static void ValidateIl2CppDumper()
        {
            if (GetContentOfFileIfExists(PublicStaticVars.il2CppDumperLocation + "version.txt") == il2CppDumperVersion) return;
            FileManager.DeleteDirectoryIfExisting(PublicStaticVars.il2CppDumperLocation);
            DownloadFile(il2CppDumperDownloadLink, "il2CppDumper.zip");
            string il2CppDumper = PublicStaticVars.tmpFolder + "il2CppDumper" + Path.DirectorySeparatorChar;
            ZipFile.ExtractToDirectory("il2CppDumper.zip", PublicStaticVars.tmpFolder + "il2CppDumper" + Path.DirectorySeparatorChar);
            Directory.Move(il2CppDumper, PublicStaticVars.il2CppDumperLocation);
            File.WriteAllText(PublicStaticVars.il2CppDumperLocation + "version.txt", il2CppDumperVersion);
        }

        public static void ValidateDexTools()
        {
            if (GetContentOfFileIfExists(PublicStaticVars.dexToolsLocation + "version.txt") == dexToolsVersion) return;
            FileManager.DeleteDirectoryIfExisting(PublicStaticVars.dexToolsLocation);
            DownloadFile(dexToolsDownloadLink, "dexTools.zip");
            string dexTools = PublicStaticVars.tmpFolder + "dexTools" + Path.DirectorySeparatorChar;
            ZipFile.ExtractToDirectory("dexTools.zip", PublicStaticVars.tmpFolder + "dexTools" + Path.DirectorySeparatorChar);
            Directory.Move(dexTools + "dex-tools-2.1", PublicStaticVars.dexToolsLocation);
            File.WriteAllText(PublicStaticVars.dexToolsLocation + "version.txt", dexToolsVersion);
        }

        public static void ValidateJdCli()
        {
            if (GetContentOfFileIfExists(PublicStaticVars.jdCliLocation + "version.txt") == jdCliVersion) return;
            FileManager.DeleteDirectoryIfExisting(PublicStaticVars.jdCliLocation);
            DownloadFile(jdCliDownloadLink, "jdCli.zip");
            ZipFile.ExtractToDirectory("jdCli.zip", PublicStaticVars.jdCliLocation);
            File.WriteAllText(PublicStaticVars.jdCliLocation+ "version.txt", jdCliVersion);
        }

        public static bool DownloadFile(string url, string destination)
        {
            DownloadProgressUI ui = new DownloadProgressUI();
            return ui.StartDownload(url, destination);
        }

        public static string GetContentOfFileIfExists(string file)
        {
            if (File.Exists(file))return File.ReadAllText(file);
            return "";
        }
    }
}
