using Firewatch.Core;
using Firewatch.Core.Logging;
using Firewatch.Database;
using Firewatch.Telemetry;

using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace Firewatch;

public static class Firewatch
{
    public static string            ConfigFile  = "firewatchConfig.xml";

    public static LoggingManager    Logger      = new LoggingManager(ConfigFile);
    private static LoggingChannel   Log;
    public static Config            Config;
    public static FirewatchContext  DB;

    public static TickProcessor     TickProcessor;

    static void Main(string[] args)
    {
        Config = new Config(ConfigFile, Logger.GetLogger("Config"));
        Log = Logger.GetLogger("Firewatch");

        DB = new FirewatchContext();
        DB.SetLogger(Logger.GetLogger("DBContext"));
        /// Since we're only operating the local DB right now - we can use programmatic migrations method to remove an extra-step with
        /// creating and running a bundle prior to application start. We'll make sure the DB is up to date on every application start-up
        Log.Debug("Starting DB migration");
        RelationalDatabaseFacadeExtensions.Migrate(DB.Database);
        Log.Debug("DB migration finished");

        TickProcessor = new TickProcessor(Logger.GetLogger("TickProcessor"));
        TickProcessor.InitializeTimer();

        /// TODO: There's a potential to add an interactive console. It's not needed for the time being, so we simply use
        /// unprocessed ReadLine to keep the process running;
        while(true) {
            string command = Console.ReadLine();
            if (command == "exit")
                break;
        }
    }
}