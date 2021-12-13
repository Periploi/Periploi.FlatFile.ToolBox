using Periploi.FlatFile.ToolBox.Positional;

namespace Periploi.FlatFile.ToolBox.Tests.Models
{
    internal class EmptyModelContainer : PositionalRecord
    {
        public EmptyModelContainer(string line, int lineNumber) : base(line, lineNumber)
        {
        }

        public override PositionalRecordDefinition Definition { get; } = new PositionalRecordDefinition();
    }
}