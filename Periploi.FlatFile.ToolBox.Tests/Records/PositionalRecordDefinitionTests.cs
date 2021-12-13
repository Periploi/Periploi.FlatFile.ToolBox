﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Periploi.FlatFile.ToolBox.Positional;
using Periploi.FlatFile.ToolBox.Tests.Fields;
using Shouldly;

namespace Periploi.FlatFile.ToolBox.Tests.Records
{
    [TestClass]
    public class PositionalRecordDefinitionTests
    {
        [TestMethod]
        public void CreateDefaultPositionalRecordDefinitionAndValidateValues()
        {
            var arrange1 = PositionalFieldTests.Arrange();

            var assert = new PositionalRecordDefinition();

            assert.FieldsCollection.Fields.ShouldBeEmpty();
            assert.PossibleChilds.ShouldBeEmpty();
            assert.MinLength.ShouldBe(-1);
            assert.MaxLength.ShouldBe(int.MaxValue);
            assert.IsFixedSize.ShouldBeTrue();
            assert.TagName.ShouldBeNullOrEmpty();
            assert.GetWhiteSpaceRecord().ShouldBe(string.Empty);

            //var act = PositionalFieldMap.Create(arrangeFieldDefinitions);

            //act.Fields.Length.ShouldBe(arrangeFieldDefinitions.Length);
            //act.ExpectedTotalLength.ShouldBe(arrangeFieldDefinitions.Length * Length);
        }
    }
}