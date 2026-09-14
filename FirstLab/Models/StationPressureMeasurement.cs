namespace FirstLab.Models;

public class StationPressureMeasurement : PressureMeasurement
{
    public string StationName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsAutomatic { get; set; }

    public StationPressureMeasurement(
        DateTime dateMeasure,
        double height,
        int value,
        string stationName,
        double latitude,
        double longitude,
        bool isAutomatic)
        : base(dateMeasure, height, value)
    {
        StationName = stationName;
        Latitude = latitude;
        Longitude = longitude;
        IsAutomatic = isAutomatic;
    }

    protected override string TypeName => $"Pressure from Station {StationName}";

    protected override string FormatExtraFields()
    {
        return $"""
                Latitude: {Latitude}
                Longitude: {Longitude}
                IsAutomatic: {IsAutomatic}
                """;
    }
}
