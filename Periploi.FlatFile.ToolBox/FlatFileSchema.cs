namespace Periploi.FlatFile.ToolBox;

[Serializable]
public sealed class FlatFileSchema
{
    public string Name { get; set; }
    public List<FlatFileRecord> Records { get; set; } = new();
}

[Serializable]
public sealed class FlatFileRecord
{
    public string Name { get; set; }
    public string XPath { get; set; }
    public string Tag { get; set; }
    public string Type { get; set; }
    public int MinOccurs { get; set; } = 1;
    public int MaxOccurs { get; set; } = 1;
    public List<FlatFileRecord> Records { get; set; } = new();
    public List<FlatFileRecordField> Fields { get; set; } = new();
}

[Serializable]
public sealed class FlatFileRecordField
{
    public string Name { get; set; }
    public int Length { get; set; }
    public string XPath { get; set; }
    public string Type { get; set; }
    public string Justification { get; set; }
    public int MinOccurs { get; set; } = 1;
    public int MaxOccurs { get; set; } = 1;
}