namespace Periploi.FlatFile.ToolBox.Core.Positional;

public interface IRecord<TC>
    where TC : FlatFileContext, new()
{
    /// <summary>
    ///     The factory used to create this instance.
    /// </summary>
    RecordFactory<TC> Factory { get; }

    /// <summary>
    ///     This record definition reference.
    /// </summary>
    RecordDefinition<TC> Definition { get; }

    /// <summary>
    ///     Length of the line used to create this record instance.
    /// </summary>
    int LineLength { get; }

    /// <summary>
    ///     Line number of input file, used to have context aware error messages.
    /// </summary>
    int LineNumber { get; }

    /// <summary>
    ///     Span of line chars.
    /// </summary>
    ReadOnlySpan<char> Span { get; }

    /// <summary>
    ///     Record tag, usually a 2 letter code.
    /// </summary>
    string Tag { get; }

    /// <summary>
    ///     Xml generation custom code.
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="context"></param>
    void AsXml(XmlRecord xml, TC context);

    /// <summary>
    ///     Try to instantiate a known child record from a given line.
    /// </summary>
    /// <param name="line"></param>
    /// <param name="lineIndex"></param>
    /// <param name="throwIfNotFound"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    Record<TC> GetRecord(string line, int lineIndex, bool throwIfNotFound = true);

    /// <summary>
    ///     Get his record definition (fields, children)
    /// </summary>
    /// <returns>
    ///     <see cref="RecordDefinition{TC}" />
    /// </returns>
    RecordDefinition<TC> GetDefinition();

    /// <summary>
    ///     Validate line length.
    ///     Throw <exception cref="ApplicationException"></exception> if invalid by default.
    /// </summary>
    /// <param name="throwIfInvalid">Defaults to true.</param>
    /// <returns>
    ///     <see cref="bool" />
    /// </returns>
    /// <exception cref="ApplicationException"></exception>
    bool IsValidLength(bool throwIfInvalid = true);

    /// <summary>
    ///     Get field value by index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns>
    ///     <see cref="string" />
    /// </returns>
    string GetFieldValue(int index);

    /// <summary>
    ///     Factory hydration.
    /// </summary>
    /// <param name="line"></param>
    /// <param name="lineNumber"></param>
    /// <param name="factory"></param>
    void Build(string line, int lineNumber, RecordFactory<TC> factory);
}