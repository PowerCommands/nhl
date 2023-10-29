using System.Drawing;
using PainKiller.PowerCommands.Configuration.Enums;
using PainKiller.PowerCommands.ReadLine;

namespace PainKiller.PowerCommands.Core.Services;
public static class DialogService
{
    public static bool YesNoDialog(string question, string yesValue = "y", string noValue = "n")
    {
        Console.Write($"\n{question} ({yesValue}/{noValue}): ");
        var response = Console.ReadLine();
        return $"{response}".Trim().ToLower() == yesValue.ToLower();
    }
    public static string QuestionAnswerDialog(string question)
    {
        Console.WriteLine($"\n{question}");
        Console.Write(ConfigurationGlobals.Prompt);
        var response = Console.ReadLine();
        return $"{response}".Trim();
    }

    private static string[]? Labels = null;

    public static void DrawToolbar(string[] labels, ConsoleColor[]? consoleColors = null)
    {
        if(Labels != null)  ClearToolbar();
        Labels = labels;
        var colors = consoleColors ?? new[] { ConsoleColor.Green, ConsoleColor.DarkRed, ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow, ConsoleColor.DarkGray,ConsoleColor.Red,ConsoleColor.DarkBlue,ConsoleColor.DarkGreen,ConsoleColor.Blue,ConsoleColor.Cyan,ConsoleColor.Magenta,ConsoleColor.Yellow,ConsoleColor.DarkBlue,ConsoleColor.Green, ConsoleColor.DarkRed, ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow, ConsoleColor.DarkGray,ConsoleColor.Red,ConsoleColor.DarkBlue,ConsoleColor.DarkGreen,ConsoleColor.Blue,ConsoleColor.Cyan,ConsoleColor.Magenta,ConsoleColor.Yellow,ConsoleColor.DarkBlue,ConsoleColor.Green, ConsoleColor.DarkRed, ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow, ConsoleColor.DarkGray,ConsoleColor.Red,ConsoleColor.DarkBlue,ConsoleColor.DarkGreen,ConsoleColor.Blue,ConsoleColor.Cyan,ConsoleColor.Magenta,ConsoleColor.Yellow,ConsoleColor.DarkBlue,ConsoleColor.Green, ConsoleColor.DarkRed, ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow, ConsoleColor.DarkGray,ConsoleColor.Red,ConsoleColor.DarkBlue,ConsoleColor.DarkGreen,ConsoleColor.Blue,ConsoleColor.Cyan,ConsoleColor.Magenta,ConsoleColor.Yellow,ConsoleColor.DarkBlue };
        colors = colors.Reverse().ToArray();
        var originalPosition = new Point(x: Console.CursorLeft, y: Console.CursorTop);
        var colorIndex = 0;
        var width = 0;
        var startBackgroundColor = Console.BackgroundColor;

        foreach (var label in labels.Reverse().Take(colors.Length))
        {
            var color = colors[colorIndex++];
            width += label.Length + 1;
            Console.SetCursorPosition(Math.Clamp(Console.WindowWidth - width, 0, Console.WindowWidth), Math.Clamp(Console.WindowHeight - 2, 0, Console.WindowHeight));
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = color;
            Console.Write(label);
            Console.BackgroundColor = startBackgroundColor;
            Console.Write(" ");
        }
        Console.ResetColor();
        Console.SetCursorPosition(originalPosition.X, originalPosition.Y);
    }
    public static void ClearToolbar()
    {
        if(Labels.Length == 0) return;
        var originalPosition = new Point(x: Console.CursorLeft, y: Console.CursorTop);
        var width = 0;
        foreach (var label in Labels) width += label.Length+1; 
        Console.SetCursorPosition( Math.Clamp(Console.WindowWidth - width, 0, Console.WindowWidth),  Math.Clamp(Console.WindowHeight-2, 0, Console.WindowHeight));
        Console.Write("".PadLeft(width,' '));
        Console.SetCursorPosition(originalPosition.X, originalPosition.Y);
    }

    private static ToolbarConfiguration? _configuration = null;
    public static void DrawToolbar(ToolbarConfiguration? configuration)
    {
        _configuration = configuration;
        if(_configuration == null ) return;
        DrawToolbar(_configuration.ToolbarItems.Select(t => t.Label).ToArray(),_configuration.ToolbarItems.Select(t => t.Color).ToArray());
        if(_configuration.HideToolbarOption == HideToolbarOption.OnTextChange) ReadLineService.CmdLineTextChanged += ReadLineService_CmdLineTextChanged;
        else if(_configuration.HideToolbarOption == HideToolbarOption.OnCommandHighlighted) ReadLineService.CommandHighlighted += ReadLineService_CommandHighlighted;
    }

    private static void ReadLineService_CommandHighlighted(object? sender, ReadLine.Events.CommandHighlightedArgs e)
    {
        if(_configuration?.ToolbarItems == null ) return;
        ClearToolbar();
        ReadLineService.CommandHighlighted -= ReadLineService_CommandHighlighted;
    }

    private static void ReadLineService_CmdLineTextChanged(object? sender, ReadLine.Events.CmdLineTextChangedArgs e)
    {
        if(_configuration?.ToolbarItems == null ) return;
        ClearToolbar();
        ReadLineService.CmdLineTextChanged -= ReadLineService_CmdLineTextChanged;
    }
}