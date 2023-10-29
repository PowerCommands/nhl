using System.IO.Compression;

namespace NhlCommands.Commands;

[PowerCommandDesign( description: "Install the data files.",
                         example: "install")]
public class InstallCommand : CommandBase<PowerCommandsConfiguration>
{
    public InstallCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var zipFilePath = Path.Combine(AppContext.BaseDirectory, "BaseDataFiles", "data.zip");
        if(File.Exists(zipFilePath)) WriteSuccessLine("Data file exists.");
        else return BadParameterError($"Data file {zipFilePath} does not exist.");

        var dataDirectory = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, "nhl");
        if(!Directory.Exists(dataDirectory)) Directory.CreateDirectory(dataDirectory);
        else
        {
            var overwrite = DialogService.YesNoDialog("This will overwrite the existing data files, do you want to continue?");
            if (!overwrite) return Ok();
        }
        ZipFile.ExtractToDirectory(zipFilePath, dataDirectory, overwriteFiles: true);
        return Ok();
    }
}