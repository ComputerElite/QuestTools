using ComputerUtils.CommandLine;
using ComputerUtils.Logging;

namespace QuestTools
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            Logger.displayLogInConsole = true;
            Logger.SetLogFile("Log.log");
            Logger.Log("Starting QuestTools version " + Menu.updater.version);
            PublicStaticVars.arguments = new CommandLineCommandContainer(args);
            Menu.StartMenu();
        }
    }
}