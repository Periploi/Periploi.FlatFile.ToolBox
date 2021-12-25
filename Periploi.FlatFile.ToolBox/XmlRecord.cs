namespace Periploi.FlatFile.ToolBox;

/// <summary>
///     Xml Helper
/// </summary>
public sealed class XmlRecord : IDisposable
{
    public XmlRecord(XmlWriter xml)
    {
        Xml = xml;
    }

    /// <summary>
    ///     The inner <see cref="XmlWriter" />
    ///     You can write directly to it, but you need to handle nodes closures by yourself if you do so.
    /// </summary>
    public XmlWriter Xml { get; private set; }

    /// <summary>
    ///     Currently opened nodes count.
    /// </summary>
    public int OpenNodesCounter { get; private set; }

    public void Dispose()
    {
        for (var i = 0; i < OpenNodesCounter; i++)
            Xml.WriteEndElement();

        Xml = null;
    }

    /// <summary>
    ///     Add a node to the stack with possibles <see cref="Action&lt;XmlRecord&gt;" />.
    /// </summary>
    /// <param name="nodeName"></param>
    /// <param name="actions"></param>
    public void OpenNode(string nodeName, params Action<XmlRecord>[] actions)
    {
        Xml.WriteStartElement(nodeName);
        OpenNodesCounter++;

        foreach (var action in actions)
            action?.Invoke(this);
    }

    /// <summary>
    ///     Open a new node context.
    /// </summary>
    /// <param name="nodeName"></param>
    /// <returns>A new <see cref="XmlRecord" /></returns>
    public XmlRecord OpenContext(string nodeName)
    {
        var context = new XmlRecord(Xml);
        context.OpenNode(nodeName);
        return context;
    }

    /// <summary>
    ///     Write node with possibles <see cref="Action&lt;XmlRecord&gt;" />.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="actions"></param>
    public void WriteNode(string name, params Action<XmlRecord>[] actions)
    {
        Xml.WriteStartElement(name);

        foreach (var action in actions)
            action?.Invoke(this);

        Xml.WriteEndElement();
    }

    /// <summary>
    ///     Write node with an inner content.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="content"></param>
    public void WriteNodeWithContent(string name, string content)
    {
        Xml.WriteStartElement(name);
        Xml.WriteString(content);
        Xml.WriteEndElement();
    }

    /// <summary>
    ///     Open node with a xmlns attribute.
    /// </summary>
    /// <param name="nodeName"></param>
    /// <param name="ns"></param>
    public void OpenNodeWithNamespace(string nodeName, string ns)
    {
        Xml.WriteStartElement(nodeName, ns);
        OpenNodesCounter++;
    }

    /// <summary>
    ///     Write an attribute on the last context's node.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void WriteAttribute(string name, string value)
    {
        Xml.WriteAttributeString(name, value);
    }
}