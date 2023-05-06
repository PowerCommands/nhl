using PainKiller.PowerCommands.Core.Services;

ConsoleService.Service.WriteHeaderLine(nameof(Program), "NHL STATS Commands 1.0");
ConsoleService.Service.WriteLine(nameof(Program), "");
ConsoleService.Service.WriteLine(nameof(Program), "General instructions");
ConsoleService.Service.WriteLine(nameof(Program), "Use download command to update the database.");
ConsoleService.Service.WriteLine(nameof(Program), "Use db command to view details about downloaded data.");
ConsoleService.Service.WriteLine(nameof(Program), "Use stats, draft, seasons, goals, points commands to view statistic.");
ConsoleService.Service.WriteLine(nameof(Program), "");
ConsoleService.Service.WriteLine(nameof(Program), "First argument is the season year (the season end year is the season year) if omitted the current season is used.");
ConsoleService.Service.WriteLine(nameof(Program), "You could add nationalities like SWE FIN to do comparision based on them.");
ConsoleService.Service.WriteLine(nameof(Program), "Example, show finnish and swedish hockey skaters stats in the top 100 by points for season 2020.");
ConsoleService.Service.WriteCodeExample(nameof(Program), "stats","2020 SWE FIN --top 100");
ConsoleService.Service.WriteLine(nameof(Program), "");
ConsoleService.Service.WriteLine(nameof(Program), "You could use tab to find suggestion for valid nationalities.");
ConsoleService.Service.WriteLine(nameof(Program), "");
PainKiller.PowerCommands.Bootstrap.Startup.ConfigureServices().Run(args);