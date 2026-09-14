using System.Globalization;
using FirstLab.Models;

namespace FirstLab;

public static class PressureParser
{
    private const string DateFormat = "dd.MM.yyyy";

    public static PressureMeasurement Parse(string line)
    {
        var parts = SplitLine(line);
        var type = parts[0].ToLowerInvariant();

        return type switch
        {
            "measure" => ParseBaseMeasurement(parts),
            "station" => ParseStationMeasurement(parts),
            _ => throw new FormatException($"Неизвестный тип измерения: '{parts[0]}'"),
        };
    }

    public static List<PressureMeasurement> ParseLines(IEnumerable<string> lines)
    {
        var result = new List<PressureMeasurement>();

        foreach (var line in lines)
        {
            if (IsBlankOrComment(line)) continue;
            result.Add(Parse(line));
        }

        return result;
    }

    private static bool IsBlankOrComment(string line)
    {
        var trimmed = line.Trim();
        return trimmed.Length == 0 || trimmed.StartsWith('#');
    }

    private static string[] SplitLine(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            throw new ArgumentNullException(nameof(line), "Данные пустые и отсутствуют");

        return line.Trim().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
    }

    private static void EnsurePartCount(string[] parts, int expected, string typeName)
    {
        if (parts.Length != expected)
        {
            throw new FormatException(
                $"Для типа '{typeName}' должно быть {expected} частей, у вас {parts.Length}: {string.Join(' ', parts)}");
        }
    }

    private static (DateTime Date, double Height, int Value) ParseCommonFields(string[] parts)
    {
        var date = ParseDate(parts[1]);
        var height = ParseHeight(parts[2]);
        var value = ParseValue(parts[3]);
        return (date, height, value);
    }

    private static DateTime ParseDate(string dateStr)
    {
        if (!DateTime.TryParseExact(dateStr, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            throw new FormatException($"Invalid date: '{dateStr}'");

        return date;
    }

    private static double ParseHeight(string heightStr)
    {
        return ParseDouble(heightStr, "height");
    }

    private static int ParseValue(string valueStr)
    {
        if (!int.TryParse(valueStr, out var value))
            throw new FormatException($"Invalid pressure: '{valueStr}'");

        return value;
    }

    private static bool ParseFlag(string flagStr, string fieldName)
    {
        if (!bool.TryParse(flagStr, out var flag))
            throw new FormatException($"Invalid {fieldName}: '{flagStr}'");

        return flag;
    }

    private static double ParseDouble(string numberStr, string fieldName)
    {
        if (!double.TryParse(numberStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var number))
            throw new FormatException($"Invalid {fieldName}: '{numberStr}'");

        return number;
    }

    private static PressureMeasurement ParseBaseMeasurement(string[] parts)
    {
        EnsurePartCount(parts, 4, "measure");
        var (date, height, value) = ParseCommonFields(parts);
        return new PressureMeasurement(date, height, value);
    }

    private static PressureMeasurement ParseStationMeasurement(string[] parts)
    {
        EnsurePartCount(parts, 8, "station");
        var (date, height, value) = ParseCommonFields(parts);

        var stationName = parts[4];
        var latitude = ParseDouble(parts[5], "latitude");
        var longitude = ParseDouble(parts[6], "longitude");
        var isAutomatic = ParseFlag(parts[7], "isAutomatic");

        return new StationPressureMeasurement(date, height, value, stationName, latitude, longitude, isAutomatic);
    }
}
