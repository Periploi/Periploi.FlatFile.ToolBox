namespace Periploi.FlatFile.ToolBox.Positional;

public static class RecordExtensions
{
    /// <summary>
    ///     Throw <exception cref="ApplicationException"></exception> with formatted message.
    /// </summary>
    /// <param name="record"></param>
    /// <param name="child"></param>
    /// <exception cref="ApplicationException"></exception>
    public static void ThrowRecordShouldBeUnique<T>(this IRecord<T> record, IRecord<T> child) where T : FlatFileContext, new()
    {
        throw new ApplicationException($"'{child.GetType().Name}' should be unique for '{record.GetType().Name}' line {record.LineNumber}");
    }


    /// <summary>
    ///     Throw <exception cref="ApplicationException"></exception> with formatted message.
    /// </summary>
    /// <param name="record"></param>
    /// <param name="child"></param>
    /// <param name="line"></param>
    /// <exception cref="ApplicationException"></exception>
    public static void ThrowUnknowChildRecord<T>(this IRecord<T> record, IRecord<T> child, string line = null) where T : FlatFileContext, new()
    {
        var possibleValues = string.Join(", ", record.Definition.ChildFactories.Keys.ToArray());
        var message =
            $"'{child?.GetType().Name ?? line?.Substring(0, 2)}' is unknow for '{record.GetType().Name}' line {record.LineNumber}{Environment.NewLine}Possibles values : {possibleValues}";
        throw new ApplicationException(message);
    }

    /// <summary>
    ///     All fields and their values.
    /// </summary>
    /// <returns>
    ///     <see cref="IEnumerable{KeyValuePair}" />
    /// </returns>
    public static IEnumerable<KeyValuePair<string, string>> GetFields<T>(this IRecord<T> record) where T : FlatFileContext, new()
    {
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var i = 0; i < record.Definition.FieldsCollection.Fields.Length; i++)
        {
            var field = record.Definition.FieldsCollection.Fields[i];
            yield return new KeyValuePair<string, string>(field.Name, record.GetFieldValue(i));
        }
    }
}