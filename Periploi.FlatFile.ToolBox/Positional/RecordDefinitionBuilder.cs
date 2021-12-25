namespace Periploi.FlatFile.ToolBox.Positional;

public class Record<T, TC> : IDisposable
    where T : Record<TC>, new()
    where TC : FlatFileContext, new()
{
    private readonly Queue<FieldDefinition> fieldDefinitions = new();
    private RecordDefinition<TC> definition;

    private Record()
    {
    }

    public void Dispose()
    {
        fieldDefinitions.Clear();
        definition = null;
    }

    public static Record<T, TC> InitRoot()
    {
        return Init(string.Empty);
    }

    public static Record<T, TC> Init(string tag)
    {
        return Init(tag ?? throw new ArgumentNullException(nameof(tag)), string.IsNullOrEmpty(tag) ? -1 : tag.Length);
    }

    private static Record<T, TC> Init(string tag, int tagLength)
    {
        var result = new Record<T, TC>
        {
            definition = new RecordDefinition<TC>
            {
                Tag = tag,
                TagLength = tagLength
            }
        };

        return result.Field("Tag", tagLength);
    }

    public Record<T, TC> Field(string name, int length)
    {
        fieldDefinitions.Enqueue(new FieldDefinition
        {
            Name = name,
            Length = length
        });

        return this;
    }

    public Record<T, TC> Property<TU>(
        string tag,
        Func<T, TU> getter,
        Action<T, TU> setter,
        ChildBehaviors behavior = ChildBehaviors.Append
    )
        where TU : Record<TC>, new()
    {
        var unique = new RecordFactory<T, TU, TC>
        {
            Tag = tag,
            ChildType = ChildTypes.Property,
            Getter = getter ?? throw new ArgumentNullException(nameof(getter)),
            Setter = setter ?? throw new ArgumentNullException(nameof(setter)),
            Append = behavior.HasFlag(ChildBehaviors.Append),
            EmitXml = behavior.HasFlag(ChildBehaviors.EmitXml)
        };
        definition.ChildFactories.Add(tag, unique);
        return this;
    }

    public Record<T, TC> List<TU>(
        string tag,
        Func<T, List<TU>> getter,
        ChildBehaviors behavior = ChildBehaviors.Append
    )
        where TU : Record<TC>, new()
    {
        var collection = new RecordFactory<T, TU, TC>
        {
            Tag = tag,
            ChildType = ChildTypes.List,
            ListGetter = getter ?? throw new ArgumentNullException(nameof(getter)),
            Append = behavior.HasFlag(ChildBehaviors.Append),
            EmitXml = behavior.HasFlag(ChildBehaviors.EmitXml)
        };

        definition.ChildFactories.Add(tag, collection);
        return this;
    }

    public RecordDefinition<TC> ToDefinition()
    {
        if (!string.IsNullOrEmpty(definition.Tag) && !fieldDefinitions.Any())
            throw new NotImplementedException();

        if (!string.IsNullOrEmpty(definition.Tag))
            definition.FieldsCollection = FieldMap.Create(fieldDefinitions.ToArray());

        try
        {
            return definition;
        }
        finally
        {
            Dispose();
        }
    }
}