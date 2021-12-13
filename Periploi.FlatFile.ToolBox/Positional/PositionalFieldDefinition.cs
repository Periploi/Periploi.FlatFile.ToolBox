namespace Periploi.FlatFile.ToolBox.Positional;

[Serializable]
public class PositionalFieldDefinition
{
    public string Name { get; set; }
    public int Length { get; set; }
    public Type Type { get; set; } = typeof(string);
}