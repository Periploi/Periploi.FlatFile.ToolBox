namespace Periploi.FlatFile.ToolBox.Core.Positional;

/// <summary>
///     Record fields collection helper.
/// </summary>
[Serializable]
public class FieldMap
{
    internal FieldMap()
    {
        Fields = new Field[] { };
    }

    private FieldMap(int count)
    {
        Fields = new Field[count];
    }

    /// <summary>
    ///     Fields collection.
    /// </summary>
    public Field[] Fields { get; private set; }

    /// <summary>
    ///     Record line expected total length, based upon the fields collection.
    /// </summary>
    public int ExpectedTotalLength { get; private set; }

    internal static FieldMap Create(FieldDefinition[] fields)
    {
        var result = new FieldMap(fields.Length);
        var startPosition = 0;
        for (var i = 0; i < fields.Length; i++)
        {
            var field = fields[i];
            result.Fields[i] = Field.Create(field, startPosition);
            startPosition += field.Length;
        }

        result.ExpectedTotalLength = startPosition;

        return result;
    }
}