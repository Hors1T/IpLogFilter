using System.Net;

namespace IPLogFilter;

public class LogProcessor
{
    private readonly string _logFilePath;

    public LogProcessor(string logFilePath)
    {
        _logFilePath = logFilePath;
    }

    public List<LogEntry> ReadLogs()
    {
        var logs = new List<LogEntry>();
        try
        {
            var lines = File.ReadAllLines(_logFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                if (parts.Length >= 2)
                {
                    var ip = IPAddress.Parse(parts[0]);
                    var time = DateTime.Parse(parts[1]);
                    logs.Add(new LogEntry(ip, time));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка чтения логов: {ex.Message}");
        }
        return logs;
    }
}