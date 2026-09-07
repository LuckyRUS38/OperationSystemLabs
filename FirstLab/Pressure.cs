namespace FirstLab;

public class Pressure(DateTime dateMeasure, double height, int value)
{ 
    DateTime DateMeasure { get; set; } = dateMeasure;
    double Height { get; set; } = height;
    int Value { get; set; } = value;

    public override string ToString()
    {
        return($"""
                           Pressure
                           Date: {DateMeasure}
                           Height: {Height}
                           Value: {Value}
                           
                           """);
    }
}