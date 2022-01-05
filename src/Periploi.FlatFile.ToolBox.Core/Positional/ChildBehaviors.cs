namespace Periploi.FlatFile.ToolBox.Core.Positional;

/// <summary>
///     Determine what to do on child creation.
/// </summary>
[Flags]
[Serializable]
public enum ChildBehaviors
{
    /// <summary>
    ///     Do nothing.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Append to the parent object.
    /// </summary>
    Append = 1,

    /// <summary>
    ///     Emit xml directly when the object is completed.
    /// </summary>
    EmitXml = 2,

    /// <summary>
    ///     Do both <see cref="Append" /> and <see cref="EmitXml" />
    /// </summary>
    All = Append | EmitXml
}