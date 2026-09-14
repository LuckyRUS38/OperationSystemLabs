using FirstLab.Models;

namespace FirstLab;

public static class MeasurementBatchLoader
{
    public static BatchParseResult ParseText(string text)
    {
        return ParseLines(text.Split('\n'));
    }

    public static BatchParseResult ParseFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Файл не найден: {path}", path);

        return ParseLines(File.ReadAllLines(path));
    }

    private static BatchParseResult ParseLines(IEnumerable<string> lines)
    {
        var measurements = new List<PressureMeasurement>();
        var errors = new List<string>();
        var lineNumber = 0;

        foreach (var rawLine in lines)
        {
            lineNumber++;
            var line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith('#')) continue;

            TryParseLine(line, lineNumber, measurements, errors);
        }

        return new BatchParseResult(measurements, errors);
    }

    private static void TryParseLine(string line, int lineNumber, List<PressureMeasurement> measurements, List<string> errors)
    {
        try
        {
            measurements.Add(PressureParser.Parse(line));
        }
        catch (Exception ex)
        {
            errors.Add($"Строка {lineNumber}: {ex.Message}");
        }
    }
}