namespace FirstLab;

internal class Program
{
    public static List<Pressure> Pressures = new List<Pressure>();
    private static void Start()
    {
        Console.WriteLine("""
                          -=-=-=-=-=-=-=-=-
                          Парсер давления (строка -> объект)
                          -=-=-=-=-=-=-=-=-
                          Введите данные в формате: measure дд.мм.гггг мм.рт(дробное) значение(целое)
                          """);
    }

    private static void OutputListPressures()
    {
        Console.WriteLine("-=-=-=-=-=-=-=-=-");
        foreach (var pressureMeasure in Pressures)
        {
            Console.WriteLine(pressureMeasure.ToString());
        }
        Console.WriteLine("-=-=-=-=-=-=-=-=-");
    }
    
    static void Main(string[] args)
    {
        Start();
        InputHandler();
    }

    private static void InputHandler()
    {
        while (true)
        {
            string input = Console.ReadLine()!;

            if (input == "exit" || input == "quit" || input == "/q")
            {
                return;
            } else if (input == "/all")
            {
                OutputListPressures();
            }
            else
            {
                var result = PressureParser.Parse(input);
                Pressures.Add(result);
            }
        }
    }
}