using Firewatch.Core;
using Firewatch.Core.Logging;
using Firewatch.Database;

using Microsoft.EntityFrameworkCore;

string configFile = "firewatchConfig.xml";

LoggingManager logger = new LoggingManager(configFile);
Config config = new Config(configFile, logger.GetLogger("Config"));

FirewatchContext db = new FirewatchContext();
db.SetLogger(logger.GetLogger("DBContext"));
/// Since we're only operating the local DB right now - we can use programmatic migrations method to remove an extra-step with
/// creating and running a bundle prior to application start. We'll assure the DB is up to date on every application start-up
RelationalDatabaseFacadeExtensions.Migrate(db.Database);