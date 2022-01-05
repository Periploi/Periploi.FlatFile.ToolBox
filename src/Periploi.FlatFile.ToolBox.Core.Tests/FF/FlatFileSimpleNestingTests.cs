using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Periploi.FlatFile.ToolBox.Core.Tests.Models;
using Shouldly;

namespace Periploi.FlatFile.ToolBox.Core.Tests.FF;

[TestClass]
public class FlatFileSimpleNestingTests
{
    private readonly XmlWriterSettings xmlSettings = new()
    {
        Encoding = Encoding.GetEncoding("utf-8"),
        Indent = true,
        IndentChars = "  "
        //CloseOutput = true,
    };

    [TestMethod]
    [DataRow("FF_Type00_SimpleNesting1", 1, 2, 2)]
    [DataRow("FF_Type00_SimpleNesting2", 2, 2, 2)]
    [DataRow("FF_Type00_SimpleNesting3", 2, 3, 2)]
    public void ParseSimpleNesting(string fileName, int type00Count, int type01Count, int type02Count)
    {
        var filePath = Path.GetFullPath(@$".\FlatFiles\{fileName}.txt");
        var outPath = Path.GetFullPath(@$".\FlatFiles\{fileName}.xml");

        if (File.Exists(outPath)) File.Delete(outPath);

        var flatFile = new SampleModelFile();
        using var sr = new StreamReader(filePath);
        using var xmlStream = File.OpenWrite(outPath);
        using (var xmlWriter = XmlWriter.Create(xmlStream, xmlSettings))
        {
            flatFile.AsXml(sr, xmlWriter);
        }

        flatFile.ShouldNotBeNull();
        flatFile.Definition.ShouldNotBeNull();
        flatFile.Type00S.ShouldNotBeNull();
        flatFile.Type00S.Count.ShouldBe(type00Count);

        foreach (var t00 in flatFile.Type00S)
        {
            t00.Definition.ShouldNotBeNull();
            t00.Type01S.ShouldNotBeNull();
            t00.Type01S.Count.ShouldBe(type01Count);

            foreach (var t01 in t00.Type01S)
            {
                t01.Definition.ShouldNotBeNull();
                t01.Type02S.ShouldNotBeNull();
                t01.Type02S.Count.ShouldBe(type02Count);
            }
        }
    }
}