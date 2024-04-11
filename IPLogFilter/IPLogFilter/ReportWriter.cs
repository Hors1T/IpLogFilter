using System.Net;

namespace IPLogFilter;

public class ReportWriter
{
    public static void WriteResults(Dictionary<IPAddress, int> ipCounts, string outputFilePath)
    {
        try
        {
            using (var writer = new StreamWriter(outputFilePath))
            {
                foreach (var pair in ipCounts)
                {
                    writer.WriteLine($"{pair.Key}: {pair.Value}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка записи результатов: {ex.Message}");
        }
    }
}