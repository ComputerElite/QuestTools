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
        
        public const string apktoolVersion = "2.9.1";
        public const string apktoolDownloadLink = "https://bitbucket.org/iBotPeaches/apktool/downloads/apktool_2.9.1.jar";
        
        public const string signerVersion = "1.3.0";
        public const string signerDownloadLink = "https://github.com/patrickfav/uber-apk-signer/releases/download/v1.3.0/uber-apk-signer-1.3.0.jar";
        public static void DownloadAndValidateDependencies()
        {
            Logger.Log("Downloading and validating dependencies");
            ValidateDexTools();
            ValidateJdCli();
            ValidateIl2CppDumper();
            ValidateApktool();
            ValidateSigner();
        }

        public static void ValidateSigner()
        {
            if (GetContentOfFileIfExists(PublicStaticVars.signerLocation + "version.txt") == signerVersion) return;
            DownloadFile(signerDownloadLink, PublicStaticVars.signerLocation);
            File.WriteAllText(PublicStaticVars.signerLocation + "version.txt", signerVersion);
        }

        public static void ValidateApktool()
        {
            if (GetContentOfFileIfExists(PublicStaticVars.apktoolLocation + "version.txt") == apktoolVersion) return;
            DownloadFile(apktoolDownloadLink, PublicStaticVars.apktoolLocation);
            File.WriteAllText(PublicStaticVars.apktoolLocation + "version.txt", apktoolVersion);
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
            File.Delete("il2CppDumper.zip");
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
            File.Delete("dexTools.zip");
        }

        public static void ValidateJdCli()
        {
            if (GetContentOfFileIfExists(PublicStaticVars.jdCliLocation + "version.txt") == jdCliVersion) return;
            FileManager.DeleteDirectoryIfExisting(PublicStaticVars.jdCliLocation);
            DownloadFile(jdCliDownloadLink, "jdCli.zip");
            ZipFile.ExtractToDirectory("jdCli.zip", PublicStaticVars.jdCliLocation);
            File.WriteAllText(PublicStaticVars.jdCliLocation+ "version.txt", jdCliVersion);
            File.Delete("jdCli.zip");
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
