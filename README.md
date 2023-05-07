# NHL Power Commands
NHL Power Commands, dig in to NHL stats, fetch current data from NHL.com.

## Prerequisites
You need support for at least .NET 6 or higher versions, you can download current SDK from here: [Microsoft .NET Download](https://dotnet.microsoft.com/en-us/download)

## Installation of the Power Commands NHL Client
There are one prepared release for **Windows x64** machines in the release [directory](release).
- Unzip the **PainKiller.NhlCommands.zip** file to your local machine, suggestion is to create a new directory named PowerCommands.Nhl in your main program directory
- Start the program **nhl.exe** and then run the command ```install``` this will unzip the base data, that will give you a good start, over time you will need to fetch updates, which is described under the section **Maintain the database with updates** the start data is up do date with season 2022/2023. 

Now you are ready to start using the NHL PowerCommands Client!

## Run solution from Visual Studio (or a IDE tool of your own choice)
This project is open source and completely free for you to use as you wish, just clone this repo and run the code in the **src** directory, be sure to mark the **PainKiller.PowerCommands.PowerCommandsConsole** as startup project before you hit **F5**.

# Help
For every commands you could always use the ```--help``` option to display help about the command.
There are some commands that belongs to the Power Commands framework, they are not described here, read more about [Power Commands on github](https://github.com/PowerCommands/PowerCommands2022) if you are interested.

# Use tab
With the tab key you can cycle through valid commands, options and suggestion that a specific command has, many commands has a filter for countries using their abbreviation, use tab to help you.

[Follow progress on twitter](https://twitter.com/PowerCommands) <img src="https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/images/Twitter.png?raw=true" alt="drawing" width="20"/>


# Commands

## stats
Show points statistic for a specific season or current season, default top count is 25

nhl>**stats** --top 5
```
Place FullName            TeamAbbrevs Nationality GamesPlayed Points Goals Assists PointsPerGame PositionCode
    1 Connor McDavid      EDM         CAN                  82    153    64      89 1,86585       C
    2 Leon Draisaitl      EDM         DEU                  80    128    52      76 1,6           C
    3 David Pastrnak      BOS         CZE                  82    113    61      52 1,37804       R
    4 Nikita Kucherov     TBL         RUS                  82    113    30      83 1,37804       R
    5 Nathan MacKinnon    COL         CAN                  71    111    42      69 1,56338       C    
```
**Options** (option always has a trailing ```--``` before option name)

*team goalies at-least-game-count name top goals assists points-per-game forward defense rookie*

**Country filters (one or as many as you want)**

*SWE|FIN|CAN|USA|CZE|SVK|DEU|AUS|CHE|SVN|NOR|DNK|NLD|BLR|LVA|FRA|AUT|GBR|UKR|HRV|LTU|KAZ|POL|NGA|BHS|ITA|RUS*

**Examples**

### Show points stats for current top 25 (default)
 ```stats```

### Show points stats for 2010, show first top 100
 ```stats 2010 --top 100```
### Show points stats for all swedish players for current season
```stats --nation swe```
### Compare swedish and finnish players for season 2016/2017 in the top 100
```stats 2017 SWE FIN --top 100```
### Show rookie points stats for current season top 25 (default)
```stats --rookie```
### Show stats for Finnish players current season in Carolina
```stats FIN --team car```
### Show defense men points stats for current season top 25 (default)
```stats --defense```
### Show stats for current season top 25 goal scorer (default)
```stats --goals```
### Show stats for current season top 25 assists (default)
```stats --assists```
### Show stats for current season top 25 points per game (default)
```stats --goals-per-game```

## draft

Fetch draft data from NHL api to build up your base data or just display drafts from the local database file. Draft has a dependency to downloaded players.

nhl>**draft** 2017 --take 5
```
2017 Nico Hischier Naters CHE Halifax  Round:1 PickOverall: 1
2017 Nolan Patrick Winnipeg CAN Brandon  Round:1 PickOverall: 2
2017 Miro Heiskanen Espoo FIN HIFK  Round:1 PickOverall: 3
2017 Cale Makar Calgary CAN Brooks  Round:1 PickOverall: 4
2017 Elias Pettersson Sundsvall SWE Timra  Round:1 PickOverall: 5
```

**Options** (option always has a trailing ```--``` before option name)

*take include-all delete*

**Country filters (one or as many as you want)**

*SWE|FIN|CAN|USA|CZE|SVK|DEU|AUS|CHE|SVN|NOR|DNK|NLD|BLR|LVA|FRA|AUT|GBR|UKR|HRV|LTU|KAZ|POL|NGA|BHS|ITA|RUS*

**Examples**
### Show draft for season 2010/2011
```draft 2010```
### Include skaters that for some reason missing in the database (probably never made it to the NHL?)
```draft 2010 --include-all```
### Delete a draft year (in case you want to download it again)
```draft --delete 1980```

## Player
Search player with filters.

**Options** (option always has a trailing ```--``` before option name)

*goalies active un-drafted*

**Country filters (one or as many as you want)**

*SWE|FIN|CAN|USA|CZE|SVK|DEU|AUS|CHE|SVN|NOR|DNK|NLD|BLR|LVA|FRA|AUT|GBR|UKR|HRV|LTU|KAZ|POL|NGA|BHS|ITA|RUS*

**Examples**
### Search "wayne gretzky"
```player "wayne gretzky"```
### Search swedish players
```player SWE```
### Search Canadian undrafted players named "wayne"
```player "wayne" CAN --un-drafted```

## Seasons

Show goal or point stats for seasons or a specific season.

nhl>**seasons** 2010 --stop 2012
```
Season  Winner        Nation WinnerPoint PointsPerGame Over99 Games Status
2009/10 Henrik Sedin  SWE            112 1,36585            4    82 Completed
2010/11 Daniel Sedin  SWE            104 1,26829            1    82 Completed
2011/12 Evgeni Malkin RUS            109 1,45333            1    82 Completed
```

**Options** (option always has a trailing ```--``` before option name)

*stop goals*

**Examples**
### Show points leader for current season
```seasons```
### Show goal leader stats for current season
```seasons --goals```
### Show points winners for seasons 2010 to 2015
```seasons 2010 --stop 2015```

# Maintain the database with updates

## download
Download data from nhl.com, skaters stats is default and does not explicit have to been set by option.

**Options** (option always has a trailing ```--``` before option name)

*goalies standings drafts find-missing-players*

**Examples**
### Download skater statistic for current season
```download```

### Download skater statistic for season 2000
```download 2000```

### Download goalies statistic for every skater seasons previously downloaded.
```download --goalies```

### Look for players that are missing and download them.
```download --find-missing-players```

### Download drafts (and prospects) from 2010 until an already existing year.
```download 2010 --drafts```

### Download NHL team standings from the min skaters year downloaded to current season
```download --standings```

## db
With the db command you can view metadata about your local NHL json file based database, below is a sample of what is shown.

```
Players
Number of players: 7034
Nationalities: CAN|FIN|USA|CZE|SWE|SVK|RUS|LVA|UKR|POL|BLR|LTU|DEU|FRA|CHE|GBR|KAZ|NOR|NGA|KOR|AUT|JAM|SVN|ITA|PRY|NLD|SRB|VEN|DNK|LBN|AUS|HRV|BHS|JPN|BRA
Last updated:2023-04-14 11:35:12
 File size: 10 MB
 ```

**Examples**

```db```

# This applications is made with the use of Power Commands!
Read more about [Power Commands on github](https://github.com/PowerCommands/PowerCommands2022) if you are interested.

[Follow progress on twitter](https://twitter.com/PowerCommands) <img src="https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/images/Twitter.png?raw=true" alt="drawing" width="20"/>
