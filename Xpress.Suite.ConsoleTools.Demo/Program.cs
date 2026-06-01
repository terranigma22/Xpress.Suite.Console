using Xpress.Suite.ConsoleTools;
using Xpress.Suite.ConsoleTools.Enums;

var console = ConsoleManager.Instance;

// Configuración
console.SetTheme(ConsoleTheme.Light);
console.SetMinLogLevel(LogLevel.Debug);

// Header
console.Header("Console Tools Demo");

// Logging
console.Info("Application started");
console.Debug("Debug information");
console.Warn("This is a warning");
console.Success("Operation completed!");
console.Fail("Something failed");

// BlankLine
console.BlankLine(2);

// Groups
console.Group("Processing Files");
console.Info("Reading configuration...");
console.Info("Loading data...");
console.GroupEnd();


// Table
var users = new[]
{
    new { Name = "John Doe", Age = 30, Email = "john@example.com" },
    new { Name = "Jane Smith", Age = 25, Email = "jane@example.com" },
    new { Name = "Bob Johnson", Age = 35, Email = "bob@example.com" }
};

console.WriteLine("\nUser List:");
console.Table(users);

// BlankLine
console.BlankLine(2);


// Timer
console.Time(() =>
{
    Thread.Sleep(1000);
    console.Info("Operation inside timer");
}, "Sleep Operation");

// BlankLine
console.BlankLine(2);

// Counter
for (int i = 0; i < 5; i++)
{
    console.Count("LoopCounter");
}
console.Info($"Final counter value: {console.GetCounter("LoopCounter")}");

console.BlankLine();

// Progress Bar
console.WriteLine("\nProcessing:");
for (int i = 0; i <= 100; i += 1)
{
    console.ProgressBar(i, 100, 30);
    Thread.Sleep(50);
}

console.BlankLine(2);

// Spinner
console.StartSpinner("Loading data...");
Thread.Sleep(3000);
console.StopSpinner("Data loaded!");

console.BlankLine(2);

// Ask
var name = console.Ask("What's your name?");
console.Success($"Hello, {name}!");

var confirmed = console.Confirm("Do you like this library?");
if (confirmed)
    console.Success("Great! 🎉");

// Elapsed time
console.Elapsed("Total execution time");


