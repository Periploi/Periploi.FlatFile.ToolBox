namespace Periploi.FlatFile.ToolBox.Positional;

[Serializable]
public sealed class PositionalField
{
    private PositionalField()
    {
    }

    public string Name { get; set; }
    public int Start { get; set; }
    public int Length { get; set; }
    public Type Type { get; set; }

    public int End => Start + Length;

    public static PositionalField Create(PositionalFieldDefinition field, int start)
    {
        return new PositionalField
        {
            Name = field.Name,
            Start = start,
            Type = field.Type,
            Length = field.Length
        };
    }
}