using PainKiller.PowerCommands.Configuration.Enums;

namespace PainKiller.PowerCommands.Configuration.DomainObjects;

public class ToolbarConfiguration
{
    public HideToolbarOption HideToolbarOption { get; set; } = HideToolbarOption.Never;
    public List<ToolbarItemConfiguration> ToolbarItems { get; set; } = new();
}