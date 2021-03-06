using ComputerUtils.ConsoleUi;
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
        public static Updater updater = new Updater("0.0.1", "https://github.com/ComputerElite/QuestTools", "QuestTools", Assembly.GetExecutingAssembly().Location);

        public static void StartMenu()
        {
            Settings.LoadSettings();
            Dependencies.DownloadAndValidateDependencies();
            while(true)
            {
                Logger.Log("YOU PIECE OF SH*T. YOU ARE NOTHING. I'M THE GOD OF CRASHS AND THIS PROGRAM AIN'T GONNA CRASH. WHAT ARE YA GONNA DO BOUT IT? I'M GONNA KICK YOUR A**ES", LoggingType.Important);
                Images.Evil();
                Logger.Log("Created by ComputerElite");
                Images.ComputerElote();
                Logger.Log("If this is the first time running the program make sure to set an output directory. Decompiling overwrites the previous results if the output directory isn't changed.", LoggingType.Important);
                string choice = ConsoleUiController.ShowMenu(new string[]
                {
                    "Decompile selected apk to selected folder (" + PublicStaticVars.settings.packageId + "; change in settings)",
                    "Decompile Unity APK",
                    "Set setting"
                });
                switch (choice)
                {
                    case "1":
                        HorizonDecompiler.DecompileAPK();
                        break;
                    case "2":
                        UnityDecompiler.SelectAndDecompileUnityAPK();
                        break;
                    case "3":
                        Images.Settings();
                        Settings.OpenSettings();
                        break;
                }
            }
            
        }
    }
}
