namespace PainKiller.PowerCommands.Core.Services;

public static class PauseService
{
    public static void Pause(ICommandLineInput input, string description = ",  before continuing the process.")
    {
        var paus = input.OptionToInt("pause", 0);
        if (paus > 0) PauseService.Pause(paus, description);
    }
    public static void Pause(int seconds = 5, string description = ",  before continuing the process.")
    {
        Console.WriteLine();
        for (int i = 0; i < seconds; i++)
        {
                OverwritePreviousLine($"Waiting {seconds-i} seconds{description}...");
                Thread.Sleep(1000);
        }
        OverwritePreviousLine($"");
        Console.WriteLine();
    }
    private static void OverwritePreviousLine(string output)
    {
        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
        var padRight = Console.BufferWidth - output.Length;
        Console.WriteLine(output.PadRight(padRight > 0 ? padRight : 0));
    }
}