using FirstLab.Models;

namespace FirstLab;

internal class Program
{
    private const string ExitCommand1 = "exit";
    private const string ExitCommand2 = "quit";
    private const string ExitCommand3 = "/q";
    private const string ListCommand = "/all";
    private const string FileCommandPrefix = "/file";
    private const string BatchCommand = "/batch";
    private const string BatchEndMarker = "/end";

    private static readonly List<PressureMeasurement> Pressures = new();

    private static void Main(string[] args)
    {
        PrintWelcome();
        RunInputLoop();
    }

    private static void PrintWelcome()
    {
        Console.WriteLine("""
                          -=-=-=-=-=-=-=-=-
                          Парсер давления (строка -> объект)
                          -=-=-=-=-=-=-=-=-
                          Форматы строк:
                            measure дд.мм.гггг высота(дробное) значение(целое)
                            station дд.мм.гггг высота значение имяСтанции широта долгота isAutomatic(true/false)
                          Команды: /all, /file <путь>, /batch, exit
                          """);
    }

    private static void RunInputLoop()
    {
        while (true)
        {
            var input = Console.ReadLine();
            if (input is null || IsExitCommand(input)) return;

            HandleCommand(input);
        }
    }

    private static bool IsExitCommand(string input)
    {
        return input is ExitCommand1 or ExitCommand2 or ExitCommand3;
    }

    private static void HandleCommand(string input)
    {
        if (input == ListCommand)
        {
            PrintAllMeasurements();
        }
        else if (input == BatchCommand)
        {
            HandleBatchInput();
        }
        else if (input.StartsWith(FileCommandPrefix))
        {
            HandleFileInput(input);
        }
        else
        {
            HandleSingleLine(input);
        }
    }

    private static void HandleSingleLine(string input)
    {
        try
        {
            Pressures.Add(PressureParser.Parse(input));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private static void HandleFileInput(string input)
    {
        var path = input[FileCommandPrefix.Length..].Trim();
        if (path.Length == 0)
        {
            Console.WriteLine($"Укажите путь: {FileCommandPrefix} <путь>");
            return;
        }

        try
        {
            ApplyBatchResult(MeasurementBatchLoader.ParseFile(path));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка чтения файла: {ex.Message}");
        }
    }

    private static void HandleBatchInput()
    {
        Console.WriteLine($"Вводите строки, для завершения введите {BatchEndMarker}");
        var lines = ReadUntilMarker(BatchEndMarker);
        ApplyBatchResult(MeasurementBatchLoader.ParseText(string.Join('\n', lines)));
    }

    private static List<string> ReadUntilMarker(string marker)
    {
        var lines = new List<string>();
        string? line;
        while ((line = Console.ReadLine()) is not null && line != marker)
        {
            lines.Add(line);
        }

        return lines;
    }

    private static void ApplyBatchResult(BatchParseResult result)
    {
        Pressures.AddRange(result.Measurements);
        Console.WriteLine($"Добавлено измерений: {result.Measurements.Count}");

        foreach (var error in result.Errors)
        {
            Console.WriteLine($"Ошибка: {error}");
        }
    }

    private static void PrintAllMeasurements()
    {
        Console.WriteLine("-=-=-=-=-=-=-=-=-");
        foreach (var pressureMeasure in Pressures)
        {
            Console.WriteLine(pressureMeasure);
        }
        Console.WriteLine("-=-=-=-=-=-=-=-=-");
    }
}
