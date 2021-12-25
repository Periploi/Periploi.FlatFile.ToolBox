namespace Periploi.FlatFile.ToolBox.Positional;

/// <summary>
///     Base flat file execution context.
///     Can be inherited, extended and used in <see cref="Record{T}.AsXml" /> overloads.
/// </summary>
[Serializable]
public class FlatFileContext
{
    public string ReceivedFileName { get; set; }
}