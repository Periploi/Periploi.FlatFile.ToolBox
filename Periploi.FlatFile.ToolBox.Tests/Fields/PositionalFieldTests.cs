using Microsoft.VisualStudio.TestTools.UnitTesting;
using Periploi.FlatFile.ToolBox.Positional;
using Shouldly;

namespace Periploi.FlatFile.ToolBox.Tests.Fields
{
    [TestClass]
    public class PositionalFieldTests
    {
        public const int Start = 0;
        public const int Length = 42;

        public static PositionalFieldDefinition Arrange1()
        {
            return new PositionalFieldDefinition
            {
                Length = Length,
                Name = "tagada",
                Type = typeof(string)
            };
        }

        public static (PositionalFieldDefinition[] fields, int start, int length) Arrange()
        {
            return (new[]
            {
                new PositionalFieldDefinition
                {
                    Length = Length,
                    Name = "tag",
                    Type = typeof(string)
                },
                Arrange1(),
                new PositionalFieldDefinition
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

            var act = PositionalField.Create(arrange, Start);

            act.Length.ShouldBe(arrange.Length);
            act.Name.ShouldBe(arrange.Name);
            act.Type.ShouldBe(arrange.Type);
            act.Start.ShouldBe(Start);
            act.End.ShouldBe(arrange.Length + Start);
        }
    }
}