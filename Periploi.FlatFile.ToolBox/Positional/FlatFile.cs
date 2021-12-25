namespace Periploi.FlatFile.ToolBox.Positional;

/// <inheritdoc cref="Record{T}" />
[Serializable]
public abstract class FlatFile<T> : Record<T>
    where T : FlatFileContext, new()
{
    /// <summary>
    ///     This file context.
    /// </summary>
    public T Context { get; internal set; } = new();

    public static TU CreateInstance<TU>(T context)
        where TU : FlatFile<T>, new()
    {
        var result = new TU
        {
            Factory = null,
            Definition = null,
            Context = context
        };
        result.Definition = result.GetDefinition();
        return result;
    }

    /// <summary>
    ///     Xml entry point.
    ///     Can be overriden if needed, but it already call <see cref="AsXml" /> from <see cref="Record{T}" /> that should be
    ///     implemented instead.
    /// </summary>
    /// <param name="sr"></param>
    /// <param name="xmlWriter"></param>
    public virtual void AsXml(StreamReader sr, XmlWriter xmlWriter)
    {
        xmlWriter.WriteStartDocument();

        using var xml = new XmlRecord(xmlWriter);
        AsXml(xml, Context);
        Process(sr, xml);
    }

    /// <summary>
    ///     Top level records handling.
    /// </summary>
    /// <param name="sr"></param>
    /// <param name="xml"></param>
    protected void Process(StreamReader sr, XmlRecord xml)
    {
        var lineIndex = 0;
        var peekResult = string.Empty;

        while (sr.Peek() >= 0 || !string.IsNullOrWhiteSpace(peekResult))
        {
            var line = GetLine(sr, ref peekResult, ref lineIndex);
            var record = GetRecord(line, lineIndex);

            if (record.Definition.ChildFactories.Any())
                peekResult = PeekRecords(sr, xml, record, ref lineIndex);

            record.Factory.Apply(xml, Context, this, record);
        }
    }

    /// <summary>
    ///     Peek lines in case record can have children.
    /// </summary>
    /// <param name="sr"></param>
    /// <param name="xml"></param>
    /// <param name="parent"></param>
    /// <param name="lineIndex"></param>
    /// <returns>peekResult as <see cref="string" /></returns>
    private string PeekRecords(TextReader sr, XmlRecord xml, Record<T> parent, ref int lineIndex)
    {
        var peekResult = string.Empty;
        var recordStack = new Stack<Record<T>>();
        recordStack.Push(parent);

        while (sr.Peek() >= 0 || !string.IsNullOrWhiteSpace(peekResult))
        {
            var line = GetLine(sr, ref peekResult, ref lineIndex);
            var currentRecord = recordStack.Pop();
            var child = currentRecord.GetRecord(line, lineIndex, false);
            if (null == child)
            {
                if (currentRecord == parent)
                    return line;

                var r = recordStack.Pop();
                currentRecord.Factory.Apply(xml, Context, r, currentRecord);
                if (r == parent) recordStack.Push(r);
                peekResult = line;
                continue;
            }

            if (child.Tag.Equals(currentRecord.Tag))
            {
                // TODO : should be possible to allow multiple pseudo child tags same as the 'parent'
            }

            if (child.Definition.ChildFactories.Any())
            {
                recordStack.Push(currentRecord);
                recordStack.Push(child);
                continue;
            }

            child.Factory.Apply(xml, Context, currentRecord, child);
            recordStack.Push(currentRecord);
        }

        var last = recordStack.Pop();
        while (recordStack.Count >= 1)
        {
            var previous = recordStack.Pop();
            last.Factory.Apply(xml, Context, previous, last);
            last = previous;
        }

        return peekResult;
    }

    private static string GetLine(TextReader sr, ref string peekResult, ref int lineIndex)
    {
        string line;
        if (string.IsNullOrWhiteSpace(peekResult))
        {
            line = sr.ReadLine();
            lineIndex++;
        }
        else
        {
            line = peekResult;
            peekResult = string.Empty;
        }

        return line;
    }
}