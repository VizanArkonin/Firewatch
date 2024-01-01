using System.Text;

namespace Firewatch.Telemetry.Monitors;

public abstract class Monitor
{
    protected static string ReadLineStartingWith(string path, string lineStartsWith)
    {
        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 512, FileOptions.SequentialScan | FileOptions.Asynchronous))
        using (var r = new StreamReader(fs, Encoding.ASCII))
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                if (line.StartsWith(lineStartsWith, StringComparison.Ordinal))
                    return line;
            }
        }

        return null;
    }
}