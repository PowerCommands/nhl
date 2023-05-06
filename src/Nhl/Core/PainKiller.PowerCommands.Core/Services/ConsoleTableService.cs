using PainKiller.PowerCommands.Shared.Utils.DisplayTable;

namespace PainKiller.PowerCommands.Core.Services;
public static class ConsoleTableService
{
    private static readonly Dictionary<string, IEnumerable<IColumnRender>> TableColumnRenderDefinitions = new();
    public static void RenderTable<T>(IEnumerable<T> items, IConsoleWriter consoleWriter) where T : new()
    {
        var tableItems = items.ToArray();
        if (!tableItems.Any()) return;
        var rows = ConsoleTable
            .From<T>(tableItems)
            .Configure(o => o.NumberAlignment = Alignment.Right)
            .Read(WriteFormat.Alternative).Split("\r\n");
        RenderConsoleTable(rows, consoleWriter);
    }
    public static void RenderConsoleCommandTable<T>(IEnumerable<T> items, IConsoleWriter consoleWriter) where T : class, IConsoleCommandTable, new()
    {
        var tableItems = items.ToArray();
        if (!tableItems.Any()) return;
        if (typeof(T).GetInterface(nameof(IConsoleCommandTable)) != null)
        {
            var consoleCommandTable = tableItems.First();
            RenderTable(tableItems, consoleCommandTable.GetColumnRenderOptionsAttribute().ToArray(), consoleWriter);
            return;
        }
        var rows = ConsoleTable
            .From<T>(tableItems)
            .Configure(o => o.NumberAlignment = Alignment.Right)
            .Read(WriteFormat.Alternative).Split("\r\n");
        RenderConsoleTable(rows, consoleWriter);
    }
    private static void RenderConsoleTable(string[] rows, IConsoleWriter consoleWriter)
    {
        for (var rowIndex = 0; rowIndex < rows.Length; rowIndex++)
        {
            if(rowIndex % 2 == 0) continue;
            var row = rows[rowIndex];
            row = row.Replace("+-", "").Replace("-+", "").Replace(" |", "").Replace("| ", "");
            if (rowIndex < 3)
            {
                var foregroundColor = Console.ForegroundColor;
                var color = Console.BackgroundColor;
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                consoleWriter.Write(row);
                Console.BackgroundColor = color;
                Console.ForegroundColor = foregroundColor;
                Console.WriteLine();
                continue;
            }
            var color2 = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            consoleWriter.Write(row);
            Console.BackgroundColor = color2;
            Console.WriteLine();
        }
    }
    public static void AddTableColumnRenderDefinitions(string name, IEnumerable<IColumnRender> columnRenderDefinitions)
    {
        if (TableColumnRenderDefinitions.ContainsKey(name)) return;
        TableColumnRenderDefinitions.Add(name, columnRenderDefinitions);
    }
    private static void RenderTable<T>(IEnumerable<T> tableData, ColumnRenderOptionsAttribute[] columnRenderDefinitions, IConsoleWriter consoleWriter)
    {
        var rows = ConsoleTable
            .From<T>(tableData)
            .Configure(o => o.NumberAlignment = Alignment.Right)
            .Read(WriteFormat.Alternative).Split("\r\n");

        consoleWriter.WriteHeadLine(rows[0]);
        consoleWriter.WriteHeadLine(rows[1]);
        consoleWriter.WriteHeadLine(rows[2]);

        var renderCols = GetColumnRenders<T>(columnRenderDefinitions, consoleWriter).ToList();

        for (var index = 0; index < rows.Length; index++)
        {
            if (index < 3) continue;
            var row = rows[index];
            if (row.StartsWith("+-"))
            {
                consoleWriter.WriteLine(row);
                continue;
            }
            var cols = row.Split('|');
            for (var colIndex = 0; colIndex < cols.Length; colIndex++)
            {
                if (colIndex == cols.Length - 1)
                {
                    consoleWriter.WriteLine("");
                    break;
                }
                var colRender = renderCols[colIndex];
                if (colIndex > 0) colRender = renderCols[colIndex];
                colRender.Write(cols[colIndex]);
            }
        }
    }
    private static IEnumerable<IColumnRender> GetColumnRenders<T>(IEnumerable<ColumnRenderOptionsAttribute> columnRenderDefinitions, IConsoleWriter consoleWriter)
    {
        var renderCol = columnRenderDefinitions.OrderBy(c => c.Order)
            .Select(optionsAttribute => optionsAttribute.RenderFormat switch
            {
                ColumnRenderFormat.None => new ColumnRenderBase(consoleWriter),
                ColumnRenderFormat.Standard => new ColumnRenderStandard(consoleWriter),
                ColumnRenderFormat.SucessOrFailure => new ColumnRenderSuccsessOrFailure(consoleWriter, optionsAttribute.Trigger1, optionsAttribute.Trigger2, optionsAttribute.Mark), _ => throw new ArgumentOutOfRangeException()
            })
            .Cast<IColumnRender>()
            .ToList();
        renderCol.Insert(0, new ColumnRenderStandard(consoleWriter));
        renderCol.Add(new ColumnRenderBase(consoleWriter));
        AddTableColumnRenderDefinitions(typeof(T).Name, renderCol);
        return renderCol;
    }
}