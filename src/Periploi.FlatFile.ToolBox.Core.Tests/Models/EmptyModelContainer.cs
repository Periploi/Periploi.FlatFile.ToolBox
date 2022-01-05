using Periploi.FlatFile.ToolBox.Core.Positional;

namespace Periploi.FlatFile.ToolBox.Core.Tests.Models;

internal sealed class EmptyModelContainer : Record<FlatFileContext>
{
    public override void AsXml(XmlRecord xml, FlatFileContext context)
    {
        throw new NotImplementedException();
    }

    public override RecordDefinition<FlatFileContext> GetDefinition()
    {
        throw new NotImplementedException();
    }
}