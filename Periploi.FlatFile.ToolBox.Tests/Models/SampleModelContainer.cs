using Periploi.FlatFile.ToolBox.Positional;

namespace Periploi.FlatFile.ToolBox.Tests.Models
{
    internal class SampleModelContainer : PositionalRecord
    {
        public SampleModelContainer(string line, int lineNumber) : base(line, lineNumber)
        {
        }

        public override PositionalRecordDefinition Definition { get; } = new PositionalRecordDefinition();
    }
}