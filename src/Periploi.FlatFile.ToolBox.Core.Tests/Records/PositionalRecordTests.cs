using Microsoft.VisualStudio.TestTools.UnitTesting;
using Periploi.FlatFile.ToolBox.Core.Tests.Models;
using Shouldly;

namespace Periploi.FlatFile.ToolBox.Core.Tests.Records;

[TestClass]
public class PositionalRecordTests
{
    [TestMethod]
    public void CreateEmptyPositionalRecordAndValidateValues()
    {
        var assert = new EmptyModelContainer();

        assert.Definition.ShouldBeNull();
        assert.LineNumber.ShouldBe(0);
        assert.LineLength.ShouldBe(0);
        assert.GetFieldValue(-1).ShouldBe(string.Empty);
        Assert.ThrowsException<NotImplementedException>(() => assert.AsXml(null, null));
        Assert.ThrowsException<NotImplementedException>(() => assert.GetDefinition());
    }
}