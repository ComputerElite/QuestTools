using System.Diagnostics;
using System.Text.RegularExpressions;
using ComputerUtils.ConsoleUi;
using ComputerUtils.FileManaging;
using ComputerUtils.Logging;

namespace QuestTools;

public class NetworkSecurityConfigSpoof
{
    public static void Spoof(string apk = "")
    {
        if (apk == "") apk = ConsoleUiController.QuestionString("apk: ");
        if (!File.Exists(apk)) return;
        PublicStaticVars.settings.packageId = Directory.GetParent(apk).Name;
        PublicStaticVars.settings._resultDirectory = Directory.GetParent(apk).Parent.FullName + Path.DirectorySeparatorChar;
        FileManager.RecreateDirectoryIfExisting(PublicStaticVars.extracedApkLocation);
        // extract apk
        Process.Start("java.exe", "-jar \"" + PublicStaticVars.apktoolLocation + "\" d \"" + apk + "\" -f -o \"" + PublicStaticVars.extracedApkLocationWithoutSlash + "\"").WaitForExit();
        // extract network security config
        string manifestLocation = PublicStaticVars.extracedApkLocation + "AndroidManifest.xml";
        string manifest = File.ReadAllText(manifestLocation);
        // change package name
        manifest = manifest.Replace(PublicStaticVars.settings.packageId, "questtools.modified." + PublicStaticVars.settings.packageId);
        string newManifest = "";
        foreach (string line in manifest.Split("\n"))
        {
            if (line.Contains(@"android:protectionLevel=""signatureOrSystem""")) continue; // skip those permissions
            newManifest += line + "\n";
        }
        File.WriteAllText(manifestLocation, newManifest);
        Regex r = new Regex("android:networkSecurityConfig=\"(@[^\"]+)\"");
        Match m = r.Match(manifest);
        if (!m.Success)
        {
            Logger.Log("Couldn't find network security config", LoggingType.Error);
            return;
        }
        string config = m.Groups[1].Value;
        string configPath = config.Replace("@", "").Replace("/", Path.DirectorySeparatorChar.ToString()) + ".xml";
        string fullConfigPath = PublicStaticVars.extracedApkLocation + "res" + Path.DirectorySeparatorChar + configPath;
        // create spoofed network security config
        File.WriteAllText(fullConfigPath, @"<?xml version=""1.0"" encoding=""utf-8""?>
                <network-security-config>
            <base-config>
            <trust-anchors>
            <certificates src=""system"" />
            <certificates src=""user"" />
            </trust-anchors>
            </base-config>
            </network-security-config>");
        // Rebuild apk with apk tool
        Process.Start("java.exe", "-jar \"" + PublicStaticVars.apktoolLocation + "\" b \"" + PublicStaticVars.extracedApkLocationWithoutSlash + "\" -f -o \"" + PublicStaticVars.settings.resultDirectory + "spoofed.apk\"").WaitForExit();
        // sign apk
        Process.Start("java.exe", "-jar \"" + PublicStaticVars.signerLocation + "\" -a \"" + PublicStaticVars.settings.resultDirectory + "spoofed.apk\"").WaitForExit();
    }
}