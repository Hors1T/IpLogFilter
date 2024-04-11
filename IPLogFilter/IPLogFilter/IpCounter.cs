using System.Net;

namespace IPLogFilter;

public class IpCounter
{
    public Dictionary<IPAddress, int> CountIPs(List<LogEntry> logs)
    {
        var ipCounts = new Dictionary<IPAddress, int>();
        foreach (var log in logs)
        {
            if (ipCounts.ContainsKey(log.IP))
            {
                ipCounts[log.IP]++;
            }
            else
            {
                ipCounts[log.IP] = 1;
            }
        }
        return ipCounts;
    }
}