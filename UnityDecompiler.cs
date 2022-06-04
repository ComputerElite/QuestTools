using ComputerUtils.ConsoleUi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestTools
{
    public class UnityDecompiler
    {
        public static void SelectAndDecompileUnityAPK()
        {
            string apk = ConsoleUiController.QuestionString("APK: ").Replace("\"", "");
            ZipArchive a = ZipFile.OpenRead(apk);
            a.GetEntry("lib/arm64-v8a/libil2cpp.so").ExtractToFile(PublicStaticVars.tmpFolder + "libill2cpp.so", true);
            a.GetEntry("assets/bin/Data/Managed/Metadata/global-metadata.dat").ExtractToFile(PublicStaticVars.tmpFolder + "global-metadata.dat", true);
            Process p = Process.Start(new ProcessStartInfo
            {
                FileName = PublicStaticVars.il2CppDumperLocation + "Il2CppDumper.exe",
                Arguments = "\"" + PublicStaticVars.tmpFolder + "libill2cpp.so\" \"" + PublicStaticVars.tmpFolder + "global-metadata.dat\" \"" + PublicStaticVars.il2CppDumperLocation + "output\"",
                WorkingDirectory = PublicStaticVars.il2CppDumperLocation,
                RedirectStandardInput = true,
                UseShellExecute = false,
            });
            p.StandardInput.BaseStream.Write(new byte[] {(byte)'x'});
            p.WaitForExit();
            Process.Start("explorer", "explorer /e,\"" + PublicStaticVars.il2CppDumperLocation + "DummyDll\"");
        }
    }
}
