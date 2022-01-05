namespace Periploi.FlatFile.ToolBox.Core.Positional;

/// <summary>
///     Possibles children types.
/// </summary>
[Serializable]
public enum ChildTypes
{
    /// <summary>
    ///     Should not exists!
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///     A field : subset of the line chars span.
    /// </summary>
    Field = 1,

    /// <summary>
    ///     A typed <see cref="Record{T}" />.
    ///     Should be unique.
    /// </summary>
    Property = 2,

    /// <summary>
    ///     A collection of typed <see cref="Record{T}" />.
    /// </summary>
    List = 4
}