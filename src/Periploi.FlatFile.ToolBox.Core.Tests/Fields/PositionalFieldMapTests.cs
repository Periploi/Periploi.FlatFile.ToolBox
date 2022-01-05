using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Periploi.FlatFile.ToolBox.Core.Tests.Fields;

[TestClass]
public class PositionalFieldMapTests
{
    [TestMethod]
    public void CreatePositionalFieldMapAndValidateValues()
    {
        var (fields, _, length) = PositionalFieldTests.Arrange();

        //var act = FieldMap.Create(fields);

        //act.Fields.Length.ShouldBe(fields.Length);
        //act.ExpectedTotalLength.ShouldBe(fields.Length * length);
    }
}