using System.Xml;
using Firewatch.Core.Logging;

namespace Firewatch.Core;

/// <summary>
/// Config handler type - used to parse, process and provide the config values to the rest of the application
/// We're sticking to good'ol XML format, since there's no real uniformity for types and formats on configs used in .NET ecosystem.
/// </summary>
public class Config
{
    private LoggingChannel log;
    public bool EnableCPUMonitoring {get; init;} = true;

    public bool EnableRAMMonitoring {get; init;} = true;

    public bool EnableNetworkMonitoring {get; init;} = true;
    public string[] NetworkInterfacesToMonitor {get; init;} = {"end0"};

    /// <summary>
    /// How often to poll the data. Default - once every 10 seconds
    /// </summary>
    public int PollingRateInSeconds {get; init;} = 10;

    public Config(string configFile, LoggingChannel loggingChannel)
    {
        log = loggingChannel;

        XmlReaderSettings readerSettings = new XmlReaderSettings();
        readerSettings.IgnoreComments = true;
        using (XmlReader reader = XmlReader.Create(configFile, readerSettings))
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(reader);

            EnableCPUMonitoring = Boolean.Parse(xDoc.SelectSingleNode("//FirewatchConfig/EnableCPUMonitoring").InnerText);
            EnableRAMMonitoring = Boolean.Parse(xDoc.SelectSingleNode("//FirewatchConfig/EnableRAMMonitoring").InnerText);
            EnableNetworkMonitoring = Boolean.Parse(xDoc.SelectSingleNode("//FirewatchConfig/EnableNetworkMonitoring").InnerText);

            string networkInterfaces = xDoc.SelectSingleNode("//FirewatchConfig/NetworkInterfacesToMonitor").InnerText;
            if (networkInterfaces != "")
                NetworkInterfacesToMonitor = networkInterfaces.Split(",");

            int pollingRate = Int16.Parse(xDoc.SelectSingleNode("//FirewatchConfig/PollingRateInSeconds").InnerText);
            if (pollingRate < 0 || pollingRate > 60)
                log.Warning("Polling rate can't be less than 0 and more than 60 seconds. Defaulting to 10 seconds");
            else
                PollingRateInSeconds = pollingRate;
        }
    }
}