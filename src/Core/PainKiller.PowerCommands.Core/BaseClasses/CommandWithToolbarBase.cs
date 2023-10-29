using PainKiller.PowerCommands.ReadLine;
using PainKiller.PowerCommands.ReadLine.Events;

namespace PainKiller.PowerCommands.Core.BaseClasses
{
    public abstract class CommandWithToolbarBase<TConfig> : CommandBase<TConfig> where TConfig : new()
    {
        protected string LatestHighlightedCommand = string.Empty;
        
        private bool _toolbarIsInitialized;
        private readonly bool _autoShowToolbar;
        private readonly ConsoleColor[]? _colors;
        protected List<string> Labels = new();
        protected PowerCommandsToolbarAttribute? ToolbarAttribute;
        protected CommandWithToolbarBase(string identifier, TConfig configuration, bool autoShowToolbar = true, ConsoleColor[]? colors = null) : base(identifier, configuration)
        {
            _autoShowToolbar = autoShowToolbar;
            _colors = colors;
            ReadLineService.CommandHighlighted += ReadLineService_CommandHighlighted;
            ToolbarAttribute = this.GetToolbarAttribute();
        }
        
        public override RunResult Run()
        {
            DialogService.ClearToolbar();
            return Ok();
        }
        protected void ReadLineService_CommandHighlighted(object? sender, CommandHighlightedArgs e)
        {
            LatestHighlightedCommand = e.CommandName;
            if (e.CommandName != Identifier) return;
            if (_autoShowToolbar) DrawToolbar();
        }
        private void InitializeToolbar()
        {
            var attribute = this.GetPowerCommandAttribute();
            var suggestions = ToolbarAttribute == null ? Labels.ToList() : ToolbarAttribute.Labels.Split(ConfigurationGlobals.ArraySplitter).ToList();
            if (suggestions.Count == 0) suggestions.AddRange(attribute.Suggestions.Split(ConfigurationGlobals.ArraySplitter));
            if (suggestions.Count == 0) return;
            Labels.Clear();
            Labels.AddRange(suggestions);
            _toolbarIsInitialized = true;
        }
        protected void DrawToolbar()
        {
            if(!_toolbarIsInitialized) InitializeToolbar();
            DialogService.DrawToolbar(Labels.ToArray(), _colors);
        }
        
    }
}