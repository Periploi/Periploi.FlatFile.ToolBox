using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Periploi.FlatFile.ToolBox.Tests.Models;
using Shouldly;

namespace Periploi.FlatFile.ToolBox.Tests.Records
{
    [TestClass]
    public class PositionalRecordTests
    {
        [TestMethod]
        public void CreateEmptyPositionalRecordAndValidateValues()
        {
            var assert = new EmptyModelContainer(string.Empty, 0);

            assert.Definition.ShouldNotBeNull();
            assert.LineNumber.ShouldBe(0);
            assert.LineLength.ShouldBe(0);
            assert.GetFieldValue(-1).ShouldBe(string.Empty);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => assert.TagName);

            //var arrange1 = PositionalFieldTests.Arrange();

            //var act = PositionalFieldMap.Create(arrangeFieldDefinitions);

            //act.Fields.Length.ShouldBe(arrangeFieldDefinitions.Length);
            //act.ExpectedTotalLength.ShouldBe(arrangeFieldDefinitions.Length * Length);
        }
    }
}