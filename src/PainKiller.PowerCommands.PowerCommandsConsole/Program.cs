using PainKiller.PowerCommands.Core.Services;
using System.Reflection;
using NhlCommands.Managers;
using PainKiller.PowerCommands.Configuration.DomainObjects;

ConsoleService.Service.WriteLine(nameof(Program)," ____   __ __  _           _____ ______   ____  ______  _____\r\n|    \\ |  |  || |         / ___/|      | /    ||      |/ ___/\r\n|  _  ||  |  || |        (   \\_ |      ||  o  ||      (   \\_ \r\n|  |  ||  _  || |___      \\__  ||_|  |_||     ||_|  |_|\\__  |\r\n|  |  ||  |  ||     |     /  \\ |  |  |  |  _  |  |  |  /  \\ |\r\n|  |  ||  |  ||     |     \\    |  |  |  |  |  |  |  |  \\    |\r\n|__|__||__|__||_____|      \\___|  |__|  |__|__|  |__|   \\___|\r\n                                                             ", ConsoleColor.Cyan);
ConsoleService.Service.WriteHeaderLine(nameof(Program),$"\nVersion {ReflectionService.Service.GetVersion(Assembly.GetExecutingAssembly())}");

var databaseManager = new DbManager(Path.Combine(ConfigurationGlobals.ApplicationDataFolder, "nhl"));
ConsoleService.Service.WriteCodeExample(nameof(Program),"Last updated", $"{databaseManager.SeasonsDb.SkaterStats.Max(s => s.Updated)}");

PainKiller.PowerCommands.Bootstrap.Startup.ConfigureServices().Run(args);