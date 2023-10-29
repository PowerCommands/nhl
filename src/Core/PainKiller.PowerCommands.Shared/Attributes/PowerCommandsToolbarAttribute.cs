using System.ComponentModel;

namespace PainKiller.PowerCommands.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class PowerCommandsToolbarAttribute : Attribute
{
    /// <summary>
    /// Set parameters for the toolbar
    /// </summary>
    /// <param name="labels">Name of the labels</param>
    /// <param name="timer">Milliseconds before the toolbar is shown, possible min (and default) is 500 and max is 5000</param>
    /// <param name="description">Description of the toolbar</param>
    /// <param name="colors">Colors, could be null to use default colors</param>
    public PowerCommandsToolbarAttribute(string labels, int timer = 500, string description = "", ConsoleColor[]? colors = null)
    {
        Labels = labels;
        Timer = timer;
        Description = description;
        Colors = colors;
    }

    [Description("Separate items with | character")]
    public string Labels { get; }
    public string Description { get; }
    public int Timer { get; }
    public ConsoleColor[]? Colors { get; }
}