namespace Xpress.Suite.Console.Models;

public class ConsoleColorScheme
{
    public ConsoleColor InfoForeground { get; set; } = ConsoleColor.White;
    public ConsoleColor InfoBackground { get; set; } = ConsoleColor.Black;

    public ConsoleColor WarnForeground { get; set; } = ConsoleColor.Yellow;
    public ConsoleColor WarnBackground { get; set; } = ConsoleColor.Black;

    public ConsoleColor ErrorForeground { get; set; } = ConsoleColor.Red;
    public ConsoleColor ErrorBackground { get; set; } = ConsoleColor.Black;

    public ConsoleColor SuccessForeground { get; set; } = ConsoleColor.Green;
    public ConsoleColor SuccessBackground { get; set; } = ConsoleColor.Black;

    public ConsoleColor DebugForeground { get; set; } = ConsoleColor.Gray;
    public ConsoleColor DebugBackground { get; set; } = ConsoleColor.Black;

    public ConsoleColor TableHeaderForeground { get; set; } = ConsoleColor.Cyan;
    public ConsoleColor TableHeaderBackground { get; set; } = ConsoleColor.Black;

    public ConsoleColor TableBorderForeground { get; set; } = ConsoleColor.DarkGray;
    public ConsoleColor TableBorderBackground { get; set; } = ConsoleColor.Black;
}

public static class ConsoleColorSchemes
{
    public static ConsoleColorScheme Default => new();

    public static ConsoleColorScheme Dark => new()
    {
        InfoForeground = ConsoleColor.White,
        WarnForeground = ConsoleColor.DarkYellow,
        ErrorForeground = ConsoleColor.DarkRed,
        SuccessForeground = ConsoleColor.DarkGreen,
        DebugForeground = ConsoleColor.Gray,
        TableHeaderForeground = ConsoleColor.Cyan
    };

    public static ConsoleColorScheme HighContrast => new()
    {
        InfoForeground = ConsoleColor.White,
        InfoBackground = ConsoleColor.Black,
        WarnForeground = ConsoleColor.Black,
        WarnBackground = ConsoleColor.Yellow,
        ErrorForeground = ConsoleColor.White,
        ErrorBackground = ConsoleColor.Red,
        SuccessForeground = ConsoleColor.Black,
        SuccessBackground = ConsoleColor.Green
    };
}


