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
    public class Dependency
    {
        public string version { get; set; } = "";
        public string downloadLink { get; set; } = "";
        public string location { get; set; } = "";
        public bool isZip { get; set; } = true;
        public bool goIntoFirstDir { get; set; } = false;
        
        public Dependency(string version, string link, string loc, bool isZip = false, bool goIntoFirstDir = false)
        {
            this.version = version;
            this.downloadLink = link;
            this.location = loc;
            this.isZip = isZip;
            this.goIntoFirstDir = goIntoFirstDir;
        }

        public void Validate()
        {
            if (isZip)
            {
                
                if (GetContentOfFileIfExists(location + "version.txt") == version) return;
                FileManager.DeleteDirectoryIfExisting(location);
                DownloadFile(downloadLink, "tmp.zip");
                string extractDir = PublicStaticVars.tmpFolder + "tmp" + Path.DirectorySeparatorChar;
                ZipFile.ExtractToDirectory("tmp.zip", extractDir);
                if (goIntoFirstDir)
                {
                    extractDir = Directory.GetDirectories(extractDir)[0] + Path.DirectorySeparatorChar;
                }
                Directory.Move(extractDir, location);
                File.WriteAllText(location + "version.txt", version);
                File.Delete("tmp.zip");
            }
            else
            {
                if (GetContentOfFileIfExists(location + "version.txt") == version) return;
                DownloadFile(downloadLink, PublicStaticVars.apktoolLocation);
                File.WriteAllText(location + "version.txt", version);
            }
        }

        public static bool DownloadFile(string url, string destination)
        {
            DownloadProgressUI ui = new DownloadProgressUI();
            return ui.StartDownload(url, destination, true, true, new Dictionary<string, string>
            {
                {
                    "User-Agent", "QuestTools/1.0"
                }
            });
        }

        public static string GetContentOfFileIfExists(string file)
        {
            if (File.Exists(file))return File.ReadAllText(file);
            return "";
        }
    }
    public class Dependencies
    {
        public static void DownloadAndValidateDependencies()
        {
            List<Dependency> dependencies = new List<Dependency>
            {
                new Dependency("2.1", "https://github.com/pxb1988/dex2jar/releases/download/v2.1/dex2jar-2.1.zip",
                    PublicStaticVars.dexToolsLocation, true),
                new Dependency("1.6.6",
                    "https://github.com/intoolswetrust/jd-cli/releases/download/jd-cli-1.2.0/jd-cli-1.2.0-dist.zip",
                    PublicStaticVars.jdCliLocation, true),
                new Dependency("6.7.19",
                    "https://github.com/Perfare/Il2CppDumper/releases/download/v6.7.19/Il2CppDumper-net6-v6.7.19.zip",
                    PublicStaticVars.il2CppDumperLocation, true),
                new Dependency("2.9.1", "https://bitbucket.org/iBotPeaches/apktool/downloads/apktool_2.9.1.jar",
                    PublicStaticVars.apktoolLocation, false),
                new Dependency("1.3.0",
                    "https://github.com/patrickfav/uber-apk-signer/releases/download/v1.3.0/uber-apk-signer-1.3.0.jar",
                    PublicStaticVars.signerLocation, false),
                new Dependency("1.0.0", "https://codeload.github.com/vm03/payload_dumper/zip/refs/heads/master", PublicStaticVars.payloadDumperLocation, true, true)
            };
            Logger.Log("Downloading and validating dependencies");
            foreach (Dependency d in dependencies)
            {
                d.Validate();
            }
        }
    }
}
