namespace Periploi.FlatFile.ToolBox.Positional;

[Serializable]
public class PositionalFieldMap
{
    private PositionalFieldMap()
    {
    }

    private PositionalFieldMap(int count)
    {
        Fields = new PositionalField[count];
    }

    public static PositionalFieldMap Default { get; } = new(0);
    public PositionalField[] Fields { get; private set; }
    public int ExpectedTotalLength { get; private set; }

    public static PositionalFieldMap Create(PositionalFieldDefinition[] fields)
    {
        var result = new PositionalFieldMap(fields.Length);
        var startPosition = 0;
        for (var i = 0; i < fields.Length; i++)
        {
            var field = fields[i];
            result.Fields[i] = PositionalField.Create(field, startPosition);
            startPosition += field.Length;
        }

        result.ExpectedTotalLength = startPosition;

        return result;
    }
}