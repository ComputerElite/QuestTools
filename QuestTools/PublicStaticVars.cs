using ComputerUtils.ADB;
using ComputerUtils.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestTools
{
    public class PublicStaticVars
    {
        public static ADBInteractor interactor = new ADBInteractor();
        public static Settings settings = new Settings();
        public static string extracedHorizonLocation { get { return settings.resultDirectory + "extracted" + Path.DirectorySeparatorChar; } }
        public static string extracedApkLocation { get { return settings.resultDirectory + "apk" + Path.DirectorySeparatorChar; } }
        public static string extracedApkLocationWithoutSlash { get { return settings.resultDirectory + "apk"; } }
        public static string horizonJarLocation { get { return settings.resultDirectory + "jar" + Path.DirectorySeparatorChar; } }
        public static string horizonSourceLocation { get { return settings.resultDirectory + "source" + Path.DirectorySeparatorChar; } }
        public static string dexToolsLocation { get { return AppDomain.CurrentDomain.BaseDirectory + "dexTools" + Path.DirectorySeparatorChar; } }
        public static string il2CppDumperLocation { get { return AppDomain.CurrentDomain.BaseDirectory + "il2CppDumper" + Path.DirectorySeparatorChar; } }
        public static string jdCliLocation { get { return AppDomain.CurrentDomain.BaseDirectory + "jdCli" + Path.DirectorySeparatorChar; } }
        public static string apktoolLocation { get { return AppDomain.CurrentDomain.BaseDirectory + "apktool.jar"; } }
        public static string signerLocation { get { return AppDomain.CurrentDomain.BaseDirectory + "uber-apk-signer.jar"; } }
        public static string tmpFolder { get { return settings.resultDirectory + "tmp" + Path.DirectorySeparatorChar; } }
        public static CommandLineCommandContainer arguments = null;
    }
}
