using Microsoft.VisualStudio.TestTools.UnitTesting;
using Periploi.FlatFile.ToolBox.Core.Positional;

namespace Periploi.FlatFile.ToolBox.Core.Tests.Fields;

[TestClass]
public class PositionalFieldTests
{
    public const int Start = 0;
    public const int Length = 42;

    internal static FieldDefinition Arrange1()
    {
        return new FieldDefinition
        {
            Length = Length,
            Name = "tagada",
            Type = typeof(string)
        };
    }

    internal static (FieldDefinition[] fields, int start, int length) Arrange()
    {
        return (new[]
        {
            new FieldDefinition
            {
                Length = Length,
                Name = "tag",
                Type = typeof(string)
            },
            Arrange1(),
            new FieldDefinition
            {
                Length = Length,
                Name = "tsoinTsoin",
                Type = typeof(string)
            }
        }, Start, Length);
    }

    [TestMethod]
    public void CreatePositionalFieldAndValidateValues()
    {
        var arrange = Arrange1();

        //var act = Field.Create(arrange, Start);

        //act.Length.ShouldBe(arrange.Length);
        //act.Name.ShouldBe(arrange.Name);
        //act.Type.ShouldBe(arrange.Type);
        //act.Start.ShouldBe(Start);
        //act.End.ShouldBe(arrange.Length + Start);
    }
}