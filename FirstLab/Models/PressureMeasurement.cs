namespace FirstLab.Models;

public class PressureMeasurement
{
    public DateTime DateMeasure { get; set; }
    public double Height { get; set; }
    public int Value { get; set; }

    public PressureMeasurement(DateTime dateMeasure, double height, int value)
    {
        DateMeasure = dateMeasure;
        Height = height;
        Value = value;
    }

    protected virtual string TypeName => "Pressure";

    protected virtual string FormatExtraFields()
    {
        return string.Empty;
    }

    public override string ToString()
    {
        var extraFields = FormatExtraFields();
        var extraFieldsBlock = extraFields.Length == 0 ? string.Empty : extraFields + Environment.NewLine;

        return $"""
                {TypeName}
                Date: {DateMeasure:dd.MM.yyyy}
                Height: {Height}
                Value: {Value}
                {extraFieldsBlock}
                """;
    }
}
