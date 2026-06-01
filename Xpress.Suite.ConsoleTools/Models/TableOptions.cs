namespace Xpress.Suite.ConsoleTools.Models;

public class TableOptions
{
    public char BorderChar { get; set; } = '|';
    public char HorizontalSeparator { get; set; } = '-';
    public char CornerChar { get; set; } = '+';
    public bool ShowHeaders { get; set; } = true;
    public bool ShowBorder { get; set; } = true;
    public int MinColumnWidth { get; set; } = 5;
    public int MaxColumnWidth { get; set; } = 50;
    public ConsoleColor? HeaderColor { get; set; } = ConsoleColor.Cyan;
    public ConsoleColor? BorderColor { get; set; } = ConsoleColor.DarkGray;
}
