namespace Periploi.FlatFile.ToolBox.Tests.Models;

public class SampleModelFile : FlatFile<FlatFileContext>
{
    public List<Type00> Type00S { get; } = new();

    public override void AsXml(XmlRecord xml, FlatFileContext context)
    {
        xml.OpenNode("Root");
    }

    public override RecordDefinition<FlatFileContext> GetDefinition()
    {
        return Record<SampleModelFile, FlatFileContext>.InitRoot()
            .List("00", container => container.Type00S,
                ChildBehaviors.All
            )
            .ToDefinition();
    }

    public class Type00 : Record<FlatFileContext>
    {
        public List<Type01> Type01S { get; } = new();
        public Type03 Type03 { get; set; }

        public override void AsXml(XmlRecord xml, FlatFileContext context)
        {
            xml.WriteNode(GetType().Name, type00 =>
            {
                Type03?.AsXml(type00, context);

                foreach (var type01 in Type01S)
                    type01.AsXml(type00, context);
            });
        }

        public override RecordDefinition<FlatFileContext> GetDefinition()
        {
            return Init<Type00>("00")
                .List("01", type00 => type00.Type01S)
                .Property("03", type00 => type00.Type03,
                    (type00, type03) => type00.Type03 = type03
                )
                .ToDefinition();
        }
    }

    public class Type01 : Record<FlatFileContext>
    {
        public List<Type02> Type02S { get; } = new();

        public override void AsXml(XmlRecord xml, FlatFileContext context)
        {
            xml.WriteNode(GetType().Name, x =>
            {
                foreach (var type02 in Type02S)
                    type02.AsXml(x, context);
            });
        }

        public override RecordDefinition<FlatFileContext> GetDefinition()
        {
            return Init<Type01>("01")
                .List("02", type01 => type01.Type02S)
                .ToDefinition();
        }
    }

    public class Type02 : Record<FlatFileContext>
    {
        public override void AsXml(XmlRecord xml, FlatFileContext context)
        {
            xml.WriteNodeWithContent(GetType().Name, $"tsoin {DateTime.Now.ToShortTimeString()}");
        }

        public override RecordDefinition<FlatFileContext> GetDefinition()
        {
            return Init<Type02>("02")
                .ToDefinition();
        }
    }

    public class Type03 : Record<FlatFileContext>
    {
        public override void AsXml(XmlRecord xml, FlatFileContext context)
        {
            xml.WriteNodeWithContent(GetType().Name, $"tagada {DateTime.Now.ToShortTimeString()}");
        }

        public override RecordDefinition<FlatFileContext> GetDefinition()
        {
            return Init<Type03>("03")
                .ToDefinition();
        }
    }
}