namespace Xpress.Suite.ConsoleTools.Models;

public class ConsoleColorScheme
{
    public ConsoleColor InfoForeground { get; set; } = ConsoleColor.Gray;
    public ConsoleColor WarnForeground { get; set; } = ConsoleColor.Yellow;
    public ConsoleColor ErrorForeground { get; set; } = ConsoleColor.Red;
    public ConsoleColor SuccessForeground { get; set; } = ConsoleColor.Green;
    public ConsoleColor DebugForeground { get; set; } = ConsoleColor.Magenta;
    public ConsoleColor TableHeaderForeground { get; set; } = ConsoleColor.Cyan;
    public ConsoleColor TableBorderForeground { get; set; } = ConsoleColor.DarkGray;
}

public static class ConsoleColorSchemes
{
    public static ConsoleColorScheme Default => new();

    public static ConsoleColorScheme Dark => new()
    {
        InfoForeground = ConsoleColor.Gray,
        WarnForeground = ConsoleColor.DarkYellow,
        ErrorForeground = ConsoleColor.DarkRed,
        SuccessForeground = ConsoleColor.DarkGreen,
        DebugForeground = ConsoleColor.Magenta,
        TableHeaderForeground = ConsoleColor.Cyan
    };

    public static ConsoleColorScheme Light => new()
    {
        InfoForeground = ConsoleColor.Gray,
        WarnForeground = ConsoleColor.DarkYellow,
        ErrorForeground = ConsoleColor.DarkRed,
        SuccessForeground = ConsoleColor.DarkGreen,
        DebugForeground = ConsoleColor.Magenta,
        TableHeaderForeground = ConsoleColor.Cyan
    };
}


