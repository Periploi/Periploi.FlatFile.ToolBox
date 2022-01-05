namespace Periploi.FlatFile.ToolBox.Core.Positional;

[Serializable]
public class RecordDefinition<TC> where TC : FlatFileContext, new()
{
    public string Tag { get; internal set; } = string.Empty;
    public int TagLength { get; internal set; } = 2;
    public bool IsFixedSize { get; internal set; } = true;

    public Dictionary<string, RecordFactory<TC>> ChildFactories { get; internal set; }
        = new(StringComparer.InvariantCultureIgnoreCase);

    public FieldMap FieldsCollection { get; internal set; } = new();
    public int MaxLength { get; internal set; } = int.MaxValue;
    public int MinLength { get; internal set; } = -1;

    public string GetWhiteSpaceRecord()
    {
        return !IsFixedSize
            ? Tag
            : $"{Tag}".PadRight(FieldsCollection.ExpectedTotalLength, ' ');
    }
}