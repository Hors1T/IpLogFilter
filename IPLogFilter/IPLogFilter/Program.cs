using System.Net;
namespace IPLogFilter;

static class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Получить опции из аргументов командной строки
            var options = Options.GetOptions(args);

            // Обработать лог-файл
            var logProcessor = new LogProcessor(options.LogFilePath);
            var logs = logProcessor.ReadLogs();

            // Отфильтровать логи
            var filteredLogs = LogFilter.FilterLogs(logs, options);

            // Подсчитать обращения
            var ipCounter = new IpCounter();
            var ipCounts = ipCounter.CountIPs(filteredLogs);

            // Записать результаты в файл
            ReportWriter.WriteResults(ipCounts, options.OutputFilePath);

            Console.WriteLine("Готово!");
        }
        catch (Exception ex)
        {
            // ignored
        }
    }
}
