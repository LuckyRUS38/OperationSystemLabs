using System.Globalization;

namespace FirstLab;

public class PressureParser
{
    public static Pressure Parse(string line)
    {
        if (string.IsNullOrWhiteSpace(line)) 
            throw new ArgumentNullException("Данные пустые и отсутствуют:", nameof(line));
        
        var parts = line.Trim().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 4) throw new FormatException($"Должно быть 4 части (тип, дата, высота, измерение), у вас {parts.Length}: {line}");

        var dateStr = parts[1];
        var heightStr = parts[2];
        var pressureStr = parts[3];
        
        
        if (!DateTime.TryParseExact(
                dateStr,
                "dd.MM.yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
        {
            throw new FormatException($"Invalid date: '{dateStr}'");
        }

        if (!double.TryParse(heightStr, out var height))
            throw new FormatException($"Invalid height: '{heightStr}'");

        if (!int.TryParse(pressureStr, out var pressure))
            throw new FormatException($"Invalid pressure: '{pressureStr}'");

        return new Pressure(date, height, pressure);
    }
}