using ComputerUtils.ConsoleUi;
using ComputerUtils.FileManaging;
using ComputerUtils.Logging;
using ComputerUtils.Updating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuestTools
{
    public class Menu
    {
        public static Updater updater = new Updater("0.0.5", "https://github.com/ComputerElite/QuestTools", "QuestTools", Assembly.GetExecutingAssembly().Location);

        public static void StartMenu()
        {
            if(PublicStaticVars.arguments.HasArgument("--update"))
            {
                updater.Update();
                return;
            }
            updater.UpdateAssistant();
            Settings.LoadSettings();
            Dependencies.DownloadAndValidateDependencies();
            while(true)
            {
                FileManager.CreateDirectoryIfNotExisting(PublicStaticVars.tmpFolder);
                Logger.Log("YOU PIECE OF SH*T. YOU ARE NOTHING. I'M THE GOD OF CRASHS AND THIS PROGRAM AIN'T GONNA CRASH. WHAT ARE YA GONNA DO BOUT IT? I'M GONNA KICK YOUR A**ES", LoggingType.Important);
                Images.Evil();
                Logger.Log("Created by ComputerElite");
                Images.ComputerElote();
                Logger.Log("If this is the first time running the program make sure to set an output directory. Decompiling overwrites the previous results if the output directory isn't changed.", LoggingType.Important);
                string choice = ConsoleUiController.ShowMenu(new string[]
                {
                    "Decompile java app to selected folder (" + PublicStaticVars.settings.packageId + " will be pulled from your quest; change in settings)",
                    "Decompile java app to selected folder (select apk)",
                    "Decompile Unity APK (" + PublicStaticVars.settings.packageId + " will be pulled from your quest; change in settings)",
                    "Decompile Unity APK (select apk)",
                    "Set setting",
                    "Decompile all oculus apps to selected folder",
                    "Spoof network security config and recompile apk (select apk)"
                });
                switch (choice)
                {
                    case "1":
                        HorizonDecompiler.DecompileAPK(true);
                        break;
                    case "2":
                        HorizonDecompiler.DecompileAPK(false);
                        break;
                    case "3":
                        UnityDecompiler.SelectAndDecompileUnityAPK(true);
                        break;
                    case "4":
                        UnityDecompiler.SelectAndDecompileUnityAPK(false);
                        break;
                    case "5":
                        Images.Settings();
                        Settings.OpenSettings();
                        break;
                    case "6":
                        OculusDecompiler.DecompileAll();
                        break;
                    case "7":
                        NetworkSecurityConfigSpoof.Spoof();
                        break;
                }
            }
            
        }
    }
}
