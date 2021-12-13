namespace Periploi.FlatFile.ToolBox.Positional;

[Serializable]
public class PositionalRecordDefinition
{
    public string TagName { get; set; } = string.Empty;
    public int TagLength { get; set; } = 2;
    public bool IsFixedSize { get; set; } = true;

    public Dictionary<string, Func<string, int, PositionalRecord>> PossibleChilds { get; }
        = new(StringComparer.InvariantCultureIgnoreCase);

    public PositionalFieldMap FieldsCollection { get; set; } = PositionalFieldMap.Default;
    public int MaxLength { get; set; } = int.MaxValue;
    public int MinLength { get; set; } = -1;

    public string GetWhiteSpaceRecord()
    {
        return !IsFixedSize
            ? TagName
            : $"{TagName}".PadRight(FieldsCollection.ExpectedTotalLength, ' ');
    }
}