namespace Periploi.FlatFile.ToolBox.Positional;

[Serializable]
public class PositionalFlatFile<T>
    where T : PositionalRecord, new()
{
    protected T RootContainer;

    public virtual void AsXml(StreamReader sr, XmlWriter xmlWriter)
    {
        xmlWriter.WriteStartDocument();

        using var xml = new XmlWriterWrapper(xmlWriter);
        Process(sr, xml);
    }

    protected T Process(StreamReader sr, XmlWriterWrapper xml)
    {
        RootContainer ??= new T();
        var lineIndex = 0;
        var processResult = string.Empty;

        while (sr.Peek() >= 0 || !string.IsNullOrWhiteSpace(processResult))
        {
            string line;
            if (string.IsNullOrWhiteSpace(processResult))
            {
                line = sr.ReadLine();
                lineIndex++;
            }
            else
            {
                line = processResult;
                processResult = string.Empty;
            }

            var record = RootContainer.GetRecord(line, lineIndex);

            if (record.Definition.PossibleChilds.Any())
                processResult = ProcessRecordOnNextLine(sr, xml, record, ref lineIndex);

            RootContainer.OnRecordCreated(xml, record);
        }

        return RootContainer;
    }

    private string ProcessRecordOnNextLine(TextReader sr, XmlWriterWrapper xml, PositionalRecord parent,
        ref int lineIndex)
    {
        while (sr.Peek() >= 0)
        {
            var line = sr.ReadLine();
            lineIndex++;

            var record = parent.GetRecord(line, lineIndex, false);
            if (null == record)
                return line;

            if (record.Definition.PossibleChilds.Any())
                return ProcessRecordOnNextLine(sr, xml, parent, ref lineIndex);

            parent.OnRecordCreated(xml, record);
        }

        return string.Empty;
    }
}