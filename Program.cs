using System.Linq;

using Firewatch.Core;
using Firewatch.Core.Logging;
using Firewatch.Database;
using Firewatch.Database.Models;

string configFile = "firewatchConfig.xml";

LoggingManager logger = new LoggingManager(configFile);
Config config = new Config(configFile, logger.GetLogger("Config"));

FirewatchContext db = new FirewatchContext();
db.SetLogger(logger.GetLogger("DBContext"));