using Firewatch.Core;
using Firewatch.Core.Logging;
using Firewatch.Database;

using Microsoft.EntityFrameworkCore;

namespace Firewatch;

public static class Firewatch
{
    public static string configFile = "firewatchConfig.xml";

    private static LoggingManager logger = new LoggingManager(configFile);
    public static Config config;
    public static FirewatchContext db;

    static void Main(string[] args)
    {
        config = new Config(configFile, logger.GetLogger("Config"));

        db = new FirewatchContext();
        db.SetLogger(logger.GetLogger("DBContext"));
        /// Since we're only operating the local DB right now - we can use programmatic migrations method to remove an extra-step with
        /// creating and running a bundle prior to application start. We'll assure the DB is up to date on every application start-up
        RelationalDatabaseFacadeExtensions.Migrate(db.Database);
    }
}