namespace Periploi.FlatFile.ToolBox.Positional;

/// <summary>
///     A record field definition.
///     Used internally to generate <see cref="RecordDefinition" /> fields collection.
/// </summary>
[Serializable]
internal class FieldDefinition
{
    public string Name { get; set; }
    public int Length { get; set; }
    public Type Type { get; set; } = typeof(string);
}