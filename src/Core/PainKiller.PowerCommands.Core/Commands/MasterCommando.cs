namespace PainKiller.PowerCommands.Core.Commands;

public class MasterCommando : CdCommand
{
    private readonly string _alias;
    protected MasterCommando(string identifier, CommandsConfiguration configuration, string alias = "") : base(identifier, configuration) => _alias = alias;
    public override RunResult Run()
    {
        ShellService.Service.Execute(string.IsNullOrEmpty(_alias) ? Input.Identifier : _alias, Input.Raw.Replace($"{Input.Identifier} ","").Replace("--no-quit",""), WorkingDirectory, ReadLine, "", waitForExit: true);
        WriteLine($"{LastReadLine}");
        return Ok();
    }
}