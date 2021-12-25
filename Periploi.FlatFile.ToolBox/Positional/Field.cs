namespace Periploi.FlatFile.ToolBox.Positional;

/// <summary>
///     Define a record field.
/// </summary>
[Serializable]
public sealed class Field
{
    private Field()
    {
    }

    /// <summary>
    ///     Indicative name, will be used internally and returned as label.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Span start position.
    /// </summary>
    public int Start { get; set; }

    /// <summary>
    ///     Span length.
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    ///     Field type.
    ///     Note : only string is implemented for now..
    /// </summary>
    public Type Type { get; set; }

    /// <summary>
    ///     Span end position.
    /// </summary>
    public int End => Start + Length;

    internal static Field Create(FieldDefinition field, int start)
    {
        return new Field
        {
            Name = field.Name,
            Start = start,
            Type = field.Type,
            Length = field.Length
        };
    }
}