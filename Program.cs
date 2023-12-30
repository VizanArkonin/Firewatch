using Firewatch.Core;
using Firewatch.Core.Logging;

string configFile = "firewatchConfig.xml";

LoggingManager logger = new LoggingManager(configFile);
Config config = new Config(configFile, logger.GetLogger("Config"));