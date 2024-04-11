using System.Net;
using System.Net.Sockets;
using static System.String;

namespace IPLogFilter;

public class Options
{
    [CommandLine.Option('f', "file-log", Required = true, HelpText = "Путь к файлу с логами.")]
    public string LogFilePath { get; set; }

    [CommandLine.Option('o', "file-output", Required = true, HelpText = "Путь к файлу с результатом.")]
    public string OutputFilePath { get; set; }

    [CommandLine.Option('s', "address-start", HelpText = "Нижняя граница диапазона адресов.")]
    public string AddressStart { get; set; }

    [CommandLine.Option('m', "address-mask", HelpText = "Маска подсети.")]
    public int? AddressMask
    {
        get => IsNullOrEmpty(AddressStart) ? null : _addressMask;
        set => _addressMask = value;
    }

    private int? _addressMask;

    [CommandLine.Option('t', "time-start",
        HelpText = "Дата и время начала периода, за который нужно отфильтровать логи.")]
    public DateTime? TimeStart { get; set; }

    [CommandLine.Option('e', "time-end", HelpText = "Дата и время конца периода, за который нужно отфильтровать логи.")]
    public DateTime? TimeEnd { get; set; }

    public static Options GetOptions(string[] args)
    {
        var options = new Options();
        
        if (args.Length <= 0)
        {
            Console.WriteLine("Ошибка: аргументы командной строки не были переданы.");
            Console.WriteLine("Используйте --help для отображения справки.");
            return null;
        }
        
        var parser = new CommandLine.Parser(config => config.HelpWriter = Console.Out);
        var parserResult = parser.ParseArguments<Options>(args);
        
        if (parserResult.Tag != CommandLine.ParserResultType.Parsed)
        {
            Console.WriteLine("Ошибка при парсинге аргументов командной строки:");
            Console.WriteLine(parserResult.Errors.First().ToString());
            Console.WriteLine("Используйте --help для отображения справки.");
            return null;
        }
        
        options = ((CommandLine.Parsed<Options>)parserResult).Value;
        if (!IsValidIpAddress(options.AddressStart))
        {
            Console.WriteLine("Ошибка: неверный формат IP-адреса начала диапазона.");
            return null;
        }

        return options;
    }

    private static bool IsValidIpAddress(string ipAddress)
    {
        if (string.IsNullOrEmpty(ipAddress))
            return false;

        try
        {
            var address = IPAddress.Parse(ipAddress);
            return address.AddressFamily == AddressFamily.InterNetwork;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}