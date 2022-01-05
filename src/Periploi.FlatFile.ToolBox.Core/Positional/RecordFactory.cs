namespace Periploi.FlatFile.ToolBox.Core.Positional;

/// <inheritdoc cref="RecordFactory{TC}" />
public sealed class RecordFactory<T, TU, TC> : RecordFactory<TC>
    where T : Record<TC>, new()
    where TU : Record<TC>, new()
    where TC : FlatFileContext, new()
{
    internal RecordFactory()
    {
    }

    public Func<T, TU> Getter { get; internal set; }
    public Action<T, TU> Setter { get; internal set; }

    public Func<T, List<TU>> ListGetter { get; internal set; }
    public bool Append { get; internal set; }
    public bool EmitXml { get; internal set; }

    public override Record<TC> CreateRecord(string line, int lineNumber)
    {
        var result = new TU();
        Definition ??= result.GetDefinition();

        result.Build(line, lineNumber, this);
        return result;
    }

    public override void Apply(XmlRecord xml, FlatFileContext context, IRecord<TC> record, IRecord<TC> child)
    {
        var t = record as T ?? throw new ArgumentNullException(nameof(record));
        var tu = child as TU ?? throw new ArgumentNullException(nameof(child));
        var tc = context as TC ?? throw new ArgumentNullException(nameof(context));
        Apply(xml, t, tu, tc);
    }

    private void Apply(XmlRecord xml, T record, TU child, TC context)
    {
        if (Append)
            switch (ChildType)
            {
                case ChildTypes.Property:
                    if (null != Getter.Invoke(record))
                    {
                        record.ThrowRecordShouldBeUnique(child);
                        return;
                    }

                    Setter.Invoke(record, child);
                    break;
                case ChildTypes.List:
                    var list = ListGetter.Invoke(record);
                    list.Add(child);

                    break;
                case ChildTypes.Unknown:
                case ChildTypes.Field:
                default:
                    throw new ArgumentOutOfRangeException(nameof(ChildType), ChildType.ToString());
            }

        if (EmitXml)
            child.AsXml(xml, context);
    }
}