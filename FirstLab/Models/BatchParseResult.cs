namespace FirstLab.Models;

public record BatchParseResult(List<PressureMeasurement> Measurements, List<string> Errors);