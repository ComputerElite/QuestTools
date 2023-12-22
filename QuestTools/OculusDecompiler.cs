using ComputerUtils.Logging;

namespace QuestTools;

public class OculusDecompiler
{
    public static void DecompileAll()
    {
        List<string> packages = ADBHelpers.GetPackages();
        packages = packages.Where(x => x.Contains("com.oculus")).ToList();
        Logger.Log("Decompiling " + packages.Count + " packages");
        foreach (string package in packages)
        {
            Logger.Log("Decompiling " + package, LoggingType.Important);
            PublicStaticVars.settings.packageId = package.Replace("\r", "").Replace("\n", "");
            HorizonDecompiler.DecompileAPK(true);
            if (Directory.GetFiles(PublicStaticVars.horizonSourceLocation.Substring(0, PublicStaticVars.horizonSourceLocation.Length - 1)).Length == 0 && Directory.GetDirectories(PublicStaticVars.horizonSourceLocation.Substring(0, PublicStaticVars.horizonSourceLocation.Length - 1)).Length == 0)
            {
                Directory.Delete(PublicStaticVars.settings.resultDirectory, true);
            }
        }
    }
}