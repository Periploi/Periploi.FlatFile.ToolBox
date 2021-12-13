using Microsoft.VisualStudio.TestTools.UnitTesting;
using Periploi.FlatFile.ToolBox.Positional;
using Shouldly;

namespace Periploi.FlatFile.ToolBox.Tests.Fields
{
    [TestClass]
    public class PositionalFieldMapTests
    {
        [TestMethod]
        public void CreatePositionalFieldMapAndValidateValues()
        {
            var (fields, _, length) = PositionalFieldTests.Arrange();

            var act = PositionalFieldMap.Create(fields);

            act.Fields.Length.ShouldBe(fields.Length);
            act.ExpectedTotalLength.ShouldBe(fields.Length * length);
        }
    }
}