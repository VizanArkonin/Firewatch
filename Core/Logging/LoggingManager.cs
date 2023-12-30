using System.Xml;
using Firewatch.Core.Utils;

namespace Firewatch.Core.Logging;

public class LoggingManager
{
    private Dictionary<string, LoggingChannel>  loggingChannels                                 = new Dictionary<string, LoggingChannel>();
    private LoggingChannel                      log                     { get; set; }
    public string                               LogsDirectory           { get; private set; }   = "";
    private string                              logFilePrefix           { get; set; }           = "";
    private int                                 logFileSizeLimit        { get; set; }           = 1024;
    private int                                 globalLogLevelOverride  { get; set; }           = 0;
    private int                                 currentFileSize         { get; set; }           = 0;
    private bool                                writeToFile             { get; set; }           = false;
    private bool                                fileLocked              { get; set; }           = false;

    private string                              currentLogFileName      { get; set; }           = "";

    // Since there was a notable loss of data saved to file, we use queue to keep things uniform
    private Queue<LogMessage>                   messagesToProcess       { get; set; }           = new Queue<LogMessage>();

    public LoggingManager(string configFileName)
    {
        try
        {
            log = new LoggingChannel("LoggingManager", LoggingLevel.DEBUG, this);
            // Since we create our own channel hard-coded style - we manually add existing instance to the dict of channels.
            loggingChannels["LoggingManager"] = log;
            // Then we read the values from config file
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;
            using (XmlReader reader = XmlReader.Create(configFileName, readerSettings))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);

                globalLogLevelOverride = Int16.Parse(xmlDoc.SelectSingleNode("//FirewatchConfig/Logger/GlobalLogLevelOverride").InnerText);

                XmlNode fileNode = xmlDoc.SelectSingleNode("//FirewatchConfig/Logger/File");
                writeToFile = (Convert.ToBoolean(fileNode.Attributes.GetNamedItem("WriteToFile").Value));
                LogsDirectory = fileNode.Attributes.GetNamedItem("LogsDirectory").Value;
                logFilePrefix = fileNode.Attributes.GetNamedItem("LogfilePrefix").Value;
                logFileSizeLimit = Int32.Parse(fileNode.Attributes.GetNamedItem("MaximumLogSizeKB").Value);
                log.Info("Initializing Logger");

                foreach (XmlNode node in xmlDoc.SelectNodes("//FirewatchConfig/Logger/LoggingChannels/Channel"))
                {
                    log.Debug($"Creating log channel for {node.InnerText}");
                    LoggingLevel level = globalLogLevelOverride > 0 ? (LoggingLevel)globalLogLevelOverride : (LoggingLevel)Int32.Parse(node.Attributes.GetNamedItem("LogLevel").Value);
                    loggingChannels[node.InnerText] = new LoggingChannel(node.InnerText, level, this);
                    // If we've overriden the logging manager settings - we change the log variable reference to it.
                    if (node.InnerText == "LoggingManager")
                    {
                        log.Debug($"Overriding LoggingManager logger with one configured from file. Level - {level.ToString()}");
                        log = loggingChannels[node.InnerText];
                    }
                }
            }
        }
        catch (Exception e)
        {
            log.Error("Failed to read firewatchConfig.xml config file");
            log.Error(e.ToString());
        }
    }

    /// <summary>
    /// Finds and returns a Logging channel instance. If it doesn't exist - it will create a new one with ERROR level
    /// </summary>
    /// <param name="name">Name of the channel</param>
    /// <returns>LoggingChannel instance</returns>
    public LoggingChannel GetLogger(string name)
    {
        if (loggingChannels.ContainsKey(name))
        {
            return loggingChannels[name];
        }
        else
        {
            log.Debug($"No channel with name '{name}' was found. Creading default one with ERROR logging level");
            return new LoggingChannel(name, LoggingLevel.ERROR, this);
        }
    }

    /// <summary>
    /// Main external callable - used to receive LogMessage from log channels and put it into queue
    /// </summary>
    /// <param name="message">LogMessage to process</param>
    public void ReportLog(LogMessage message)
    {
        messagesToProcess.Enqueue(message);
        if (!fileLocked)
        {
            fileLocked = true;
            while (messagesToProcess.Count > 0)
            {
                ProcessMessage(messagesToProcess.Dequeue());
            }
            fileLocked = false;
        }
    }

    /// <summary>
    /// Main workhorse - processes LogMessage instance, picked from the queue
    /// </summary>
    /// <param name="message">Message to process</param>
    private void ProcessMessage(LogMessage message)
    {
        if (message is null)
            return;
        switch (message.Level)
        {
            case LoggingLevel.ERROR:
                PrintWithColor(message, ConsoleColor.Red);
                PrintIntoFile(message);
                break;
            case LoggingLevel.WARNING:
                PrintWithColor(message, ConsoleColor.Yellow);
                PrintIntoFile(message);
                break;
            case LoggingLevel.INFO:
                PrintWithColor(message, ConsoleColor.White);
                PrintIntoFile(message);
                break;
            case LoggingLevel.DEBUG:
                PrintWithColor(message, ConsoleColor.Gray);
                PrintIntoFile(message);
                break;
            case LoggingLevel.TRACE:
                PrintWithColor(message, ConsoleColor.Magenta);
                PrintIntoFile(message);
                break;
            default:
                PrintWithColor(message, ConsoleColor.White);
                PrintIntoFile(message);
                break;
        }
    }

    /// <summary>
    /// Formats and prints the log message into the console.
    /// </summary>
    /// <param name="message">Log message to process</param>
    /// <param name="color">ConsoleColor to be used for level, source and message itself. Differs depending on the level</param>
    private void PrintWithColor(LogMessage message, ConsoleColor color)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("{0,-24} ", message.Timestamp);
        Console.ForegroundColor = color;
        Console.Write("{0,-7} {1,-25}: {2}\n", $"[{message.Level.ToString()}]", $"({message.Source})", message.Message);
        Console.ResetColor();
    }

    /// <summary>
    /// File processor - creates directory and log file (if it doesn't exist), puts log data into it and keeps track of
    /// file's size. If it begins to exceed the limit - it creates a new file.
    /// </summary>
    /// <param name="message">LogMessage to process</param>
    private void PrintIntoFile(LogMessage message)
    {
        if (writeToFile)
        {
            if (currentLogFileName == "" || currentFileSize >= logFileSizeLimit * 1024)
            {
                Directory.CreateDirectory(LogsDirectory);
                currentLogFileName = $"{LogsDirectory}/{logFilePrefix}_{TimeUtils.UtcNow.ToString("MM-dd-yy_HH:mm:ss.ffff")}.log";
                File.Create(currentLogFileName).Close();
                currentFileSize = 0;
            }

            string msg = String.Format("{0,-24} {1,-7} {2,-25}: {3}\n", message.Timestamp, $"[{message.Level.ToString()}]", $"({message.Source})", message.Message);
            File.AppendAllText(currentLogFileName, msg);
            currentFileSize += msg.Length;
        }
    }
}

public enum LoggingLevel
{
    NONE = 0,
    ERROR = 1,
    WARNING = 2,
    INFO = 3,
    DEBUG = 4,
    TRACE = 5
}