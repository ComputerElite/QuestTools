using ComputerUtils.ConsoleUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuestTools
{
    public class Settings
    {
        public string resultDirectory { get; set; } = "";
        public string packageId { get; set; } = "com.oculus.horizon";

        public static void OpenSettings()
        {
            string c = ConsoleUiController.ShowMenu(new string[]
            {
                "Set results directory",
                "Set packageId"
            });
            switch(c)
            {
                case "1":
                    PublicStaticVars.settings.resultDirectory = SetString();
                    if (!PublicStaticVars.settings.resultDirectory.EndsWith(Path.DirectorySeparatorChar)) PublicStaticVars.settings.resultDirectory += Path.DirectorySeparatorChar;
                    break;
                case "2":
                    PublicStaticVars.settings.packageId = SetString();
                    break;
            }
            SaveSettings();
        }

        public static string SetString()
        {
            return ConsoleUiController.QuestionString("new value: ");
        }

        public static void SaveSettings()
        {
            File.WriteAllText("settings.json", JsonSerializer.Serialize(PublicStaticVars.settings));
        }

        public static void LoadSettings()
        {
            if(!File.Exists("settings.json")) SaveSettings();
            PublicStaticVars.settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText("settings.json"));
        }
    }
}
