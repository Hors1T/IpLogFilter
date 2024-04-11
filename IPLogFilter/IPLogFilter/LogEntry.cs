using System.Net;

namespace IPLogFilter;

public class LogEntry
{
    public IPAddress IP { get; private set; }
    public DateTime Time { get; private set; }

    public LogEntry(IPAddress ip, DateTime time)
    {
        IP = ip;
        Time = time;
    }
}