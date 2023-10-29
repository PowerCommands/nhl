using PainKiller.PowerCommands.Core.Services;

namespace PainKiller.PowerCommands.Bootstrap;
public partial class PowerCommandsManager
{
    private static bool _hasRunOnce = false;
    private void RunCustomCode()
    {
        if (!_hasRunOnce)
        {
            DialogService.DrawToolbar(Services.ExtendedConfiguration.StartupToolbar);
            _hasRunOnce = true;
        }
    }
}