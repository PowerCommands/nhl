namespace PainKiller.PowerCommands.ReadLine.Events;

public class CommandHighlightedArgs : EventArgs
{
    public CommandHighlightedArgs(string commandName) => CommandName = commandName;
    public string CommandName { get; }
}