using System.Diagnostics;
using System.Text;
using Xpress.Suite.ConsoleTools.Enums;
using Xpress.Suite.ConsoleTools.Models;

namespace Xpress.Suite.ConsoleTools;

public class ConsoleManager : IDisposable
{
    private static ConsoleManager? _instance;
    private readonly object _lock = new();
    private readonly Stopwatch _globalStopwatch;
    private readonly Dictionary<string, Stopwatch> _timers;
    private readonly Dictionary<string, int> _counters;
    private readonly List<string> _groups;
    private ConsoleColorScheme _colorScheme;
    private LogLevel _minLogLevel;
    private bool _disposed;

    // Singleton pattern
    public static ConsoleManager Instance => _instance ??= new ConsoleManager();

    private ConsoleManager()
    {
        // Cambiar a UTF-8
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // En Windows, intentar cambiar la fuente
        try
        {
            // Esto funciona en Windows Terminal y VS Code
            Console.InputEncoding = System.Text.Encoding.UTF8;
        }
        catch { }

        _globalStopwatch = Stopwatch.StartNew();
        _timers = new Dictionary<string, Stopwatch>();
        _counters = new Dictionary<string, int>();
        _groups = new List<string>();
        _colorScheme = ConsoleColorSchemes.Default;
        _minLogLevel = LogLevel.Info;
    }

    #region Configuration

    public void SetTheme(ConsoleTheme theme)
    {
        _colorScheme = theme switch
        {
            ConsoleTheme.Dark => ConsoleColorSchemes.Dark,
            ConsoleTheme.HighContrast => ConsoleColorSchemes.HighContrast,
            _ => ConsoleColorSchemes.Default
        };
    }

    public void SetColorScheme(ConsoleColorScheme scheme)
    {
        _colorScheme = scheme;
    }

    public void SetMinLogLevel(LogLevel level)
    {
        _minLogLevel = level;
    }

    #endregion

    #region Logging

    public void Log(string message, LogLevel level = LogLevel.Info)
    {
        if (level < _minLogLevel) return;

        var originalColor = Console.ForegroundColor;
        var originalBackground = Console.BackgroundColor;

        var (icon, color) = level switch
        {
            LogLevel.Trace      => ("🔍", ConsoleColor.DarkGray),
            LogLevel.Debug      => ("🐛", ConsoleColor.Gray),
            LogLevel.Info       => ("ℹ️", ConsoleColor.White),
            LogLevel.Warn       => ("⚠️", ConsoleColor.Yellow),
            LogLevel.Error      => ("❌", ConsoleColor.Red),
            LogLevel.Fatal      => ("💀", ConsoleColor.DarkRed),
            LogLevel.Success    => ("✅", ConsoleColor.Green),
            LogLevel.Fail       => ("⛔", ConsoleColor.Red),
            _                   => ("ℹ️", ConsoleColor.White)
        };

        // Usar el color específico del nivel o el del esquema
        var foregroundColor = level switch
        {
            LogLevel.Warn => _colorScheme.WarnForeground,
            LogLevel.Error or LogLevel.Fatal => _colorScheme.ErrorForeground,
            LogLevel.Debug => _colorScheme.DebugForeground,
            _ => color
        };

        var indent = new string(' ', _groups.Count * 2);
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        var formattedMessage = $"{indent}{icon} {timestamp} - {message}";

        lock (_lock)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = _colorScheme.InfoBackground;
            Console.WriteLine(formattedMessage);
            Console.ForegroundColor = originalColor;
            Console.BackgroundColor = originalBackground;
        }
    }

    public void Info(string message) => Log(message, LogLevel.Info);
    public void Debug(string message) => Log(message, LogLevel.Debug);
    public void Warn(string message) => Log(message, LogLevel.Warn);
    public void Error(string message) => Log(message, LogLevel.Error);
    public void Fatal(string message) => Log(message, LogLevel.Fatal);
    public void Trace(string message) => Log(message, LogLevel.Trace);
    public void Success(string message) => Log(message, LogLevel.Success);
    public void Fail(string message) => Log(message, LogLevel.Fail);

    public void Exception(Exception ex, string? customMessage = null)
    {
        var msg = customMessage != null ? $"{customMessage}: {ex.Message}" : ex.Message;
        Error(msg);
        if (!string.IsNullOrEmpty(ex.StackTrace))
            Debug(ex.StackTrace);
    }

    #endregion

    #region Write Methods

    public void Write(string message, ConsoleColor? color = null)
    {
        lock (_lock)
        {
            if (color.HasValue)
                Console.ForegroundColor = color.Value;

            Console.Write(message);

            if (color.HasValue)
                Console.ResetColor();
        }
    }

    public void WriteLine(string message, ConsoleColor? color = null)
    {
        lock (_lock)
        {
            if (color.HasValue)
                Console.ForegroundColor = color.Value;

            Console.WriteLine(message);

            if (color.HasValue)
                Console.ResetColor();
        }
    }

    public void WriteLine()
    {
        lock (_lock)
        {
            Console.WriteLine();
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            Console.Clear();
        }
    }

    #endregion

    #region Table

    public void Table<T>(IEnumerable<T> data, TableOptions? options = null)
    {
        options ??= new TableOptions();

        var properties = typeof(T).GetProperties();
        var headers = properties.Select(p => p.Name).ToArray();

        // Calcular ancho de columnas
        var columnWidths = new int[headers.Length];
        for (int i = 0; i < headers.Length; i++)
        {
            columnWidths[i] = Math.Max(headers[i].Length, options.MinColumnWidth);

            foreach (var item in data)
            {
                var value = properties[i].GetValue(item)?.ToString() ?? "";
                var width = value.Length;
                if (width > columnWidths[i] && width <= options.MaxColumnWidth)
                    columnWidths[i] = width;
                else if (width > options.MaxColumnWidth)
                    columnWidths[i] = options.MaxColumnWidth;
            }
        }

        lock (_lock)
        {
            // Dibujar borde superior
            if (options.ShowBorder)
                DrawTableBorder(columnWidths, options);

            // Dibujar encabezados
            if (options.ShowHeaders)
            {
                DrawTableRow(headers, columnWidths, options, true);
                if (options.ShowBorder)
                    DrawTableSeparator(columnWidths, options);
            }

            // Dibujar datos
            foreach (var item in data)
            {
                var rowData = properties.Select(p => p.GetValue(item)?.ToString() ?? "").ToArray();
                DrawTableRow(rowData, columnWidths, options, false);
            }

            // Dibujar borde inferior
            if (options.ShowBorder)
                DrawTableBorder(columnWidths, options);
        }
    }

    private void DrawTableBorder(int[] columnWidths, TableOptions options)
    {
        Console.Write(options.CornerChar);
        for (int i = 0; i < columnWidths.Length; i++)
        {
            Console.Write(new string(options.HorizontalSeparator, columnWidths[i] + 2));
            Console.Write(options.CornerChar);
        }
        Console.WriteLine();
    }

    private void DrawTableSeparator(int[] columnWidths, TableOptions options)
    {
        Console.Write(options.CornerChar);
        for (int i = 0; i < columnWidths.Length; i++)
        {
            Console.Write(new string(options.HorizontalSeparator, columnWidths[i] + 2));
            Console.Write(options.CornerChar);
        }
        Console.WriteLine();
    }

    private void DrawTableRow(string[] row, int[] columnWidths, TableOptions options, bool isHeader)
    {
        if (isHeader && options.HeaderColor.HasValue)
            Console.ForegroundColor = options.HeaderColor.Value;

        if (options.BorderColor.HasValue)
            Console.ForegroundColor = options.BorderColor.Value;

        Console.Write(options.BorderChar);

        for (int i = 0; i < row.Length; i++)
        {
            var value = row[i];
            if (value.Length > options.MaxColumnWidth)
                value = value[..(options.MaxColumnWidth - 3)] + "...";

            Console.Write($" {value.PadRight(columnWidths[i])} ");
            Console.Write(options.BorderChar);
        }

        Console.WriteLine();

        if (isHeader && options.HeaderColor.HasValue)
            Console.ResetColor();
        if (options.BorderColor.HasValue)
            Console.ResetColor();
    }

    #endregion

    #region Timing

    public void StartTimer(string? name = null)
    {
        var timerName = name ?? "default";
        if (!_timers.ContainsKey(timerName))
            _timers[timerName] = new Stopwatch();

        _timers[timerName].Start();
        Debug($"Timer '{timerName}' started");
    }

    public double StopTimer(string? name = null, bool logResult = true)
    {
        var timerName = name ?? "default";
        if (!_timers.TryGetValue(timerName, out var timer))
        {
            Warn($"Timer '{timerName}' not found");
            return 0;
        }

        timer.Stop();
        var elapsed = timer.Elapsed.TotalMilliseconds;

        if (logResult)
            Info($"Timer '{timerName}' completed in {elapsed:F2}ms");

        _timers.Remove(timerName);
        return elapsed;
    }

    public void Time(Action action, string? name = null)
    {
        var timerName = name ?? "operation";
        StartTimer(timerName);
        try
        {
            action();
        }
        finally
        {
            StopTimer(timerName);
        }
    }

    public async Task<double> TimeAsync(Func<Task> action, string? name = null)
    {
        var timerName = name ?? "async-operation";
        StartTimer(timerName);
        try
        {
            await action();
            return StopTimer(timerName);
        }
        catch
        {
            StopTimer(timerName);
            throw;
        }
    }

    public void Elapsed(string? message = null)
    {
        var elapsed = _globalStopwatch.Elapsed;
        var msg = message != null ? $"{message}: {elapsed.TotalSeconds:F2}s" : $"Total elapsed: {elapsed.TotalSeconds:F2}s";
        Info(msg);
    }

    public void ResetElapsed()
    {
        _globalStopwatch.Restart();
        Debug("Elapsed timer reset");
    }

    #endregion

    #region Counters

    public void Increment(string? counterName = null)
    {
        var name = counterName ?? "default";
        if (!_counters.ContainsKey(name))
            _counters[name] = 0;

        _counters[name]++;
    }

    public void Decrement(string? counterName = null)
    {
        var name = counterName ?? "default";
        if (!_counters.ContainsKey(name))
            _counters[name] = 0;

        _counters[name]--;
    }

    public void Count(string? counterName = null)
    {
        var name = counterName ?? "default";
        Increment(name);
        var value = _counters.GetValueOrDefault(name);
        Info($"[COUNT:{name}] = {value}");
    }

    public int GetCounter(string? counterName = null)
    {
        var name = counterName ?? "default";
        return _counters.GetValueOrDefault(name);
    }

    public void ResetCounter(string? counterName = null)
    {
        var name = counterName ?? "default";
        if (_counters.ContainsKey(name))
            _counters[name] = 0;
    }

    public void ResetAllCounters()
    {
        _counters.Clear();
        Debug("All counters reset");
    }

    #endregion

    #region Groups

    public void Group(string name)
    {
        _groups.Add(name);
        WriteLine($"┌─ {name} ─", ConsoleColor.Cyan);
    }

    public void GroupEnd()
    {
        if (_groups.Count > 0)
        {
            var name = _groups[^1];
            _groups.RemoveAt(_groups.Count - 1);
            WriteLine($"└─ End: {name} ─", ConsoleColor.Cyan);
        }
        WriteLine();
    }

    public void GroupCollapsed(string name)
    {
        Group(name);
        // En consola no hay colapso real, pero se puede simular con indentación
    }

    #endregion

    #region Progress Bar

    public void ProgressBar(int current, int total, int width = 50, ConsoleColor color = ConsoleColor.White, string ? message = null)
    {
        var percentage = (double)current / total;
        var filled = (int)(width * percentage);
        var empty = width - filled;

        var bar = $"[{new string('█', filled)}{new string('░', empty)}] {percentage:P0}";

        if (!string.IsNullOrEmpty(message))
            bar = $"{message} {bar}";

        lock (_lock)
        {
            var originalColor = Console.ForegroundColor;

            Console.ForegroundColor = color;

            Console.Write("\r" + bar);
            Console.ForegroundColor = originalColor;

            if (current >= total)
                Console.WriteLine();
        }
    }

    #endregion

    #region Spinner

    private readonly string[] _spinnerFrames = { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };
    private bool _spinnerRunning;
    private Task? _spinnerTask;

    public void StartSpinner(string message = "Processing...")
    {
        if (_spinnerRunning) return;

        _spinnerRunning = true;
        _spinnerTask = Task.Run(() =>
        {
            var frame = 0;
            while (_spinnerRunning)
            {
                lock (_lock)
                {
                    Console.Write($"\r{_spinnerFrames[frame]} {message}");
                }
                frame = (frame + 1) % _spinnerFrames.Length;
                Thread.Sleep(100);
            }

            lock (_lock)
            {
                Console.Write("\r" + new string(' ', message.Length + 2) + "\r");
            }
        });
    }

    public void StopSpinner(string? completionMessage = null)
    {
        _spinnerRunning = false;
        _spinnerTask?.Wait(500);

        if (!string.IsNullOrEmpty(completionMessage))
            Success(completionMessage);
    }

    #endregion

    #region Ask/Input

    public string? Ask(string question, ConsoleColor? color = null)
    {
        Write(question + " ", color ?? ConsoleColor.Cyan);
        return Console.ReadLine();
    }

    public T? Ask<T>(string question, Func<string, (bool, T)> validator, ConsoleColor? color = null)
    {
        while (true)
        {
            var input = Ask(question, color);
            if (input == null) return default;

            var (isValid, value) = validator(input);
            if (isValid)
                return value;

            Error("Invalid input. Please try again.");
        }
    }

    public string AskPassword(string question)
    {
        Write(question + " ", ConsoleColor.Cyan);

        var password = new StringBuilder();
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password.Append(key.KeyChar);
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password.Remove(password.Length - 1, 1);
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password.ToString();
    }

    public bool Confirm(string question, bool defaultValue = false)
    {
        var defaultText = defaultValue ? "Y/n" : "y/N";
        var answer = Ask($"{question} [{defaultText}]");

        if (string.IsNullOrEmpty(answer))
            return defaultValue;

        return answer.ToLower() == "y" || answer.ToLower() == "yes";
    }

    #endregion

    #region Separation

    public void Separator(char character = '-', int length = 50)
    {
        WriteLine(new string(character, length), ConsoleColor.DarkGray);
    }

    public void BlankLine(int lines = 1)
    {
        for (int i = 0; i < lines; i++)
            WriteLine();
    }

    #endregion

    #region Header/Footer

    public void Header(string title)
    {
        var character = new string('═', title.Length + 4);

        BlankLine();
        WriteLine($"╔{character}╗", ConsoleColor.Cyan);
        WriteLine($"║  {title}  ║", ConsoleColor.Cyan);
        WriteLine($"╚{character}╝", ConsoleColor.Cyan);
        BlankLine();
    }

    public void Footer(string message)
    {
        WriteLine($"─ {message} ─", ConsoleColor.DarkGray);
    }

    #endregion

    public void Dispose()
    {
        if (_disposed) return;

        _globalStopwatch.Stop();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}

// Métodos de extensión para fácil acceso
public static class ConsoleExtensions
{
    public static ConsoleManager Console => ConsoleManager.Instance;
}
