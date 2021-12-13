namespace Periploi.FlatFile.ToolBox.Positional;

[Serializable]
public abstract class PositionalRecord
{
    private readonly char[] chars;

    private PositionalRecord()
    {
    }

    protected PositionalRecord(string line, int lineNumber)
    {
        LineNumber = lineNumber;
        LineLength = line.Length;
        chars = line.ToCharArray();
    }

    public abstract PositionalRecordDefinition Definition { get; }

    public int LineLength { get; }
    public int LineNumber { get; }
    public ReadOnlySpan<char> Span => chars;

    public string TagName => GetFieldValue(0);

    /// <summary>
    ///     Try to instantiate a known child record from a given line.
    /// </summary>
    /// <param name="line"></param>
    /// <param name="lineIndex"></param>
    /// <param name="throwIfNotFound"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public PositionalRecord GetRecord(string line, int lineIndex, bool throwIfNotFound = true)
    {
        if (null == line)
            throw new ApplicationException($"Line {lineIndex} is null!");

        var recordKey = Definition.PossibleChilds.Keys.SingleOrDefault(line.StartsWith);
        switch (recordKey)
        {
            case null when throwIfNotFound:
                throw new ApplicationException($"Unknow record at line {lineIndex} : '{line}'");
            case null:
                return null;
            default:
            {
                var recordFactory = Definition.PossibleChilds[recordKey];
                var record = recordFactory.Invoke(line, lineIndex);
                record.IsValidLength();

                return record;
            }
        }
    }

    /// <summary>
    ///     Will be raised when a child is created.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="record"></param>
    /// <exception cref="ApplicationException"></exception>
    public virtual void OnRecordCreated(XmlWriterWrapper xml, PositionalRecord record)
    {
        throw new ApplicationException(
            $"'{record.GetType().Name}' is unknow for '{GetType().Name}' line {LineNumber}");
    }

    /// <summary>
    ///     Validate line length.
    ///     Throw <exception cref="ApplicationException"></exception> if invalid by default.
    /// </summary>
    /// <param name="throwIfInvalid">Defaults to true.</param>
    /// <returns>
    ///     <see cref="bool" />
    /// </returns>
    /// <exception cref="ApplicationException"></exception>
    public bool IsValidLength(bool throwIfInvalid = true)
    {
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

    /// <summary>
    ///     Throw <exception cref="ApplicationException"></exception> with formatted message.
    /// </summary>
    /// <param name="child"></param>
    /// <exception cref="ApplicationException"></exception>
    protected void ThrowRecordShouldBeUnique(PositionalRecord child)
    {
        throw new ApplicationException(
            $"'{child.GetType().Name}' should be unique for '{GetType().Name}' line {LineNumber}");
    }

    /// <summary>
    ///     Get field value by index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns>
    ///     <see cref="string" />
    /// </returns>
    public string GetFieldValue(int index)
    {
        if (index < 0)
            return Span.ToString();
        if (index >= Definition.FieldsCollection.Fields.Length)
            throw new ArgumentOutOfRangeException(
                $"Record '{GetType().Name}' at line {LineNumber} : Index {index} is outside fields boundaries.");

        var field = Definition.FieldsCollection.Fields[index];
        if (field.End > LineLength)
            throw new ArgumentOutOfRangeException(
                $"Record '{GetType().Name}' at line {LineNumber} : Field at index {index} is outside line boundaries.");

        return Span.Slice(field.Start, field.Length).ToString();
    }

    /// <summary>
    ///     All fields and their values.
    /// </summary>
    /// <returns>
    ///     <see cref="IEnumerable&lt;KeyValuePair&lt;string, string&gt;&gt;" />
    /// </returns>
    public IEnumerable<KeyValuePair<string, string>> GetFields()
    {
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var i = 0; i < Definition.FieldsCollection.Fields.Length; i++)
        {
            var field = Definition.FieldsCollection.Fields[i];
            yield return new KeyValuePair<string, string>(field.Name, GetFieldValue(i));
        }
    }
}