namespace Periploi.FlatFile.ToolBox.Core.Positional;

public abstract class RecordFactory<TC>
    where TC : FlatFileContext, new()
{
    /// <summary>
    ///     Relation to parent <see cref="ChildTypes" />
    /// </summary>
    public ChildTypes ChildType { get; internal set; }

    /// <summary>
    ///     Definition of the records to be build.
    /// </summary>
    public RecordDefinition<TC> Definition { get; internal set; }

    /// <summary>
    ///     Tag of the records to be build.
    /// </summary>
    public string Tag { get; internal set; }


    /// <summary>
    ///     Instantiate a record of given type.
    /// </summary>
    /// <param name="line"></param>
    /// <param name="lineNumber"></param>
    /// <returns>
    ///     <see cref="Record{T,TC}" />
    /// </returns>
    public abstract Record<TC> CreateRecord(string line, int lineNumber);


    /// <summary>
    ///     Apply record.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="context"></param>
    /// <param name="record"></param>
    /// <param name="child"></param>
    public abstract void Apply(XmlRecord xml, FlatFileContext context, IRecord<TC> record, IRecord<TC> child);
}