using PainKiller.PowerCommands.Shared.Extensions;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Core;

public class PowerOption
{
    public PowerOption(string attributeValue)
    {
        IsMandatory = attributeValue.Replace("!", "").IsUppercaseOnly();
        ValueIsRequired = attributeValue.StartsWith("!");
        Name = attributeValue.Replace("!", "");
    }
    public string Name { get; set; }
    public string Value { get; set; } = "";
    public string Raw => $"--{Name}";
    public bool ValueIsRequired { get; set; }
    public bool IsMandatory { get; set; }
}