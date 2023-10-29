using System.Text;

namespace PainKiller.PowerCommands.Core.Managers;
public class InputValidationManager
{
    private readonly ICommandLineInput _input;
    private readonly IConsoleCommand _command;
    private readonly Action<string> _logger;
    public InputValidationManager(IConsoleCommand command, ICommandLineInput input, Action<string> logger)
    {
        _input = input;
        _command = command;
        _logger = logger;
    }
    public InputValidationResult ValidateAndInitialize()
    {
        var retVal = new InputValidationResult();
        var messages = new StringBuilder();
        var attribute = _command.GetPowerCommandAttribute();
        var requiredArguments = attribute.Arguments.Split(ConfigurationGlobals.ArraySplitter).Where(a => a.StartsWith('!')).ToArray();
        if (_input.Arguments.Length < requiredArguments.Length)
        {
            messages.AppendLine($"Missing argument(s), required arguments is {requiredArguments.Length}");
            retVal.HasValidationError = true;
        }
        var requiredQuotes = attribute.Quotes.Split(ConfigurationGlobals.ArraySplitter).Where(q => q.StartsWith('!')).ToArray();
        if (_input.Quotes.Length < requiredQuotes.Length)
        {
            messages.AppendLine($"Missing quote(s), required quotes is {requiredQuotes.Length}");
            retVal.HasValidationError = true;
        }

        var optionInfos = attribute.Options.Split(ConfigurationGlobals.ArraySplitter).Select(f => new PowerOption(f)).ToList();
        retVal.Options.AddRange(optionInfos);
        
        foreach (var optionInfo in optionInfos.Where(f => f.ValueIsRequired || f.IsMandatory))
        {
            if (optionInfo.IsMandatory && !_input.HasOption(optionInfo.Name))
            {
                retVal.HasValidationError = true;
                messages.AppendLine($"Option [{optionInfo.Name}] is mandatory.");
            }
            optionInfo.Value = _input.GetOptionValue(optionInfo.Name);
            if (!string.IsNullOrEmpty(optionInfo.Value) || (!_input.HasOption(optionInfo.Name) && !optionInfo.IsMandatory)) continue;
            messages.AppendLine($"Option [{optionInfo.Name}] is required to have a value.");
            retVal.HasValidationError = true;
        }
        _logger.Invoke(messages.ToString());
        if (string.IsNullOrEmpty(attribute.Secrets)) return retVal;

        var requiredSecrets = attribute.Secrets.Split(ConfigurationGlobals.ArraySplitter).Where(s => s.StartsWith('!')).ToArray();
        foreach (var secretName in requiredSecrets)
        {
            var secret = IPowerCommandServices.DefaultInstance!.Configuration.Secret.Secrets.FirstOrDefault(s => s.Name.ToLower() == secretName.Replace("!","").ToLower());
            if (secret != null) continue;
            messages.AppendLine($"Secret [{secretName.Replace("!","")}] is required");
            retVal.HasValidationError = true;
        }
        _logger.Invoke(messages.ToString());
        return retVal;
    }
}