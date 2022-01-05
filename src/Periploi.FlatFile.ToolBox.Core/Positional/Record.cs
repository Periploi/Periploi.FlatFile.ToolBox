namespace Periploi.FlatFile.ToolBox.Core.Positional;

/// <inheritdoc cref="IRecord{T}" />
[Serializable]
public abstract class Record<T> : IRecord<T>
    where T : FlatFileContext, new()
{
    protected char[] Chars;

    public abstract void AsXml(XmlRecord xml, T context);

    public RecordFactory<T> Factory { get; internal set; }

    public RecordDefinition<T> Definition { get; internal set; }

    public int LineLength { get; internal set; }
    public int LineNumber { get; internal set; }
    public ReadOnlySpan<char> Span => Chars;

    public string Tag => GetFieldValue(0);

    public abstract RecordDefinition<T> GetDefinition();

    public bool IsValidLength(bool throwIfInvalid = true)
    {
        if (Tag.Length <= 0)
            return true;

        var expectedLength = Definition.FieldsCollection.ExpectedTotalLength;
        if (!Definition.IsFixedSize)
        {
            var isLengthInBoundaries = LineLength >= Definition.MinLength && LineLength <= Definition.MaxLength;
            if (!isLengthInBoundaries && throwIfInvalid)
                throw new ApplicationException(
                    $"Error on record '{GetType().Name}' at line {LineNumber} : line length is not in boundaries : {LineLength} should be between {Definition.MinLength} and {Definition.MaxLength}");

            return isLengthInBoundaries;
        }

        var isValid = LineLength == expectedLength;
        if (!isValid && throwIfInvalid)
            throw new ApplicationException(
                $"Error on record '{GetType().Name}' at line {LineNumber} : Expected length != line length : {expectedLength} != {LineLength}");

        return isValid;
    }

    public string GetFieldValue(int index)
    {
        if (index < 0 || !Definition.FieldsCollection.Fields.Any())
            return Span.ToString();
        if (index >= Definition.FieldsCollection.Fields.Length)
            throw new ArgumentOutOfRangeException($"Record '{GetType().Name}' at line {LineNumber} : Index {index} is outside fields boundaries.");

        var field = Definition.FieldsCollection.Fields[index];
        if (field.End > LineLength)
            throw new ArgumentOutOfRangeException($"Record '{GetType().Name}' at line {LineNumber} : Field at index {index} is outside line boundaries.");

        return Span.Slice(field.Start, field.Length).ToString();
    }

    public void Build(string line, int lineNumber, RecordFactory<T> factory)
    {
        LineNumber = lineNumber;
        LineLength = line.Length;
        Chars = line.ToCharArray();
        Factory = factory;
        Definition = Factory.Definition;
    }

    public Record<T> GetRecord(string line, int lineIndex, bool throwIfNotFound = true)
    {
        if (null == line)
            throw new ApplicationException($"Line {lineIndex} is null!");

        Definition ??= GetDefinition();

        var recordKey = Definition.ChildFactories.Keys.SingleOrDefault(line.StartsWith);
        switch (recordKey)
        {
            case null when throwIfNotFound:
                this.ThrowUnknowChildRecord(null, line);
                return null;
            case null:
                return null;
            default:
            {
                var recordFactory = Definition.ChildFactories[recordKey];
                var record = recordFactory.CreateRecord(line, lineIndex);
                record.IsValidLength();

                return record;
            }
        }
    }

    public void Dispose()
    {
        Definition = null;
        Factory = null;
    }

    protected Record<TU, T> InitRoot<TU>()
        where TU : Record<T>, new()
    {
        return Record<TU, T>.InitRoot();
    }

    protected Record<TU, T> Init<TU>(string tag)
        where TU : Record<T>, new()
    {
        return Record<TU, T>.Init(tag);
    }
}

///// <inheritdoc cref="IRecord{T}" />
//[Serializable]
//public abstract class Record<T> : Record, IRecord<T>
//    where T : FlatFileContext, new()
//{
//    public abstract void AsXml(XmlRecord xml, T context);

//    protected Record<TU, T> Init<TU>(string tag)
//        where TU : Record<T>, new()
//    {
//        return Record<TU, T>.Init(tag);
//    }
//}