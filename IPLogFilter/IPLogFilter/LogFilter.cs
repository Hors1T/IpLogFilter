using System.Net;

namespace IPLogFilter;

public static class LogFilter
{
    public static List<LogEntry> FilterLogs(List<LogEntry> logs, Options options)
    {
        var filteredLogs = logs.Where(log =>
        {
            if (!string.IsNullOrEmpty(options.AddressStart))
            {
                var startAddress = IPAddress.Parse(options.AddressStart);
                if (options.AddressMask.HasValue)
                {
                    if (!log.IP.GetNetworkAddress(options.AddressMask).Equals(startAddress.GetNetworkAddress(options.AddressMask)))
                        return false;
                }
                if (!IsInRange(log.IP, startAddress))
                    return false;
            }

            if (options.TimeStart.HasValue && log.Time < options.TimeStart.Value)
            {
                return false;
            }

            if (options.TimeEnd.HasValue && log.Time > options.TimeEnd.Value)
            {
                return false;
            }

            return true;
        });

        return filteredLogs.ToList();
    }
    private static IPAddress GetNetworkAddress(this IPAddress address, int? maskLength)
    {
        if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            throw new ArgumentException("IPv6 addresses are not supported.");
     
        var bytes = address.GetAddressBytes();
        var maskBytes = Enumerable.Range(0, 4)
            .Select(i => i < maskLength / 8 ? (byte)255 : (byte)(255 << (8 - maskLength % 8)))
            .ToArray();
        var networkBytes = bytes.Zip(maskBytes, (a, m) => (byte)(a & m)).ToArray();
     
        return new IPAddress(networkBytes);
    }
    private static bool IsInRange(IPAddress networkAddress, IPAddress startAddress)
    {
        var networkBytes = networkAddress.GetAddressBytes();
        var startBytes = startAddress.GetAddressBytes();
        return !networkBytes.Where((t, i) => t < startBytes[i]).Any();
    }
}