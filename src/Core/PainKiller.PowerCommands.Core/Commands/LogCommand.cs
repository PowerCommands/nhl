namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommandTest(tests: "list|view|--process git")]
[PowerCommandDesign( description: "View and manage the log",
                         options: "!process",
                     suggestions: "view|archive",
               disableProxyOutput: true,
                          example: "//View a list with all the logfiles|log|//Archive the logs into a zip file.|log archive|//View content of the current log|log view|//Filter the log show only posts matching the provided process tag, this requires that you are using process tags when logging in your command(s).|log --process created")]
public class LogCommand : CommandBase<CommandsConfiguration>
{
    public LogCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if (Input.Options.Length == 0) List();
        if (Input.SingleArgument == "archive") Archive();
        if (Input.SingleArgument == "view") View();
        if (Input.HasOption("process")) ProcessLog($"{Input.GetOptionValue("process")}");
        
        return Ok();
    }
    private void List()
    {
        DisableLog();
        var dir = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, Configuration.Log.FilePath));
        foreach (var file in dir.GetFiles("*.log").OrderByDescending(f => f.LastWriteTime)) WriteLine($"{file.Name} {file.LastWriteTime}");
        Console.WriteLine();
        WriteHeadLine("To view current logfile type log view");
        WriteCodeExample("log","view");

        Console.WriteLine();
        WriteHeadLine("To archive the logs into a zip file type log archive");
        WriteCodeExample("log","archive");

        Console.WriteLine();
        WriteHeadLine("To view a certain process log use:");
        WriteCodeExample("log","--process myProcess");
        EnableLog();
    }
    private void Archive() => WriteLine(Configuration.Log.ArchiveLogFiles());
    private void View()
    {
        DisableLog();
        foreach (var line in Configuration.Log.ToLines()) ConsoleService.Service.WriteLine(nameof(LogCommand), line);
        EnableLog();
    }
    private void ProcessLog(string processTag)
    {
        DisableLog();
        foreach (var line in Configuration.Log.GetProcessLog(processTag)) ConsoleService.Service.WriteLine(nameof(LogCommand), line);
        EnableLog();
    }
}