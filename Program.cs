// See https://aka.ms/new-console-template for more information

using System.Text;
using Karambolo.PO;
using OfficeOpenXml;
using UnrealL10NAutomationTool.StringTableGenerator;

ExcelPackage.License.SetNonCommercialPersonal("FlowerGolden");
Console.WriteLine("Hello, World!");

var parser = new POParser(new POParserSettings
{
    // PreserveHeadersOrder = true,
    StringDecodingOptions = new POStringDecodingOptions
    {
        KeepKeyStringsPlatformIndependent = true,
        KeepTranslationStringsPlatformIndependent = true,
    },
});

TextReader reader = new StreamReader("Game.po");

var result = parser.Parse(reader);
if (result.Success)
{
    var catalog = result.Catalog;
    using var package = new ExcelPackage("Test.xlsx");
    var generator = new Generator(catalog, package);
    var report = generator.Generate();

    var csvFile = new FileInfo("Test.csv");
    var format = new ExcelOutputTextFormat()
    {
        Delimiter = ',',
        Encoding = Encoding.UTF8,
    };
    package.Workbook.Worksheets[0].Cells[1, 1, report.Num + 2, 2].SaveToText(csvFile, format);
    package.Save();

    var reportFile = new StreamWriter("report.txt");
    await reportFile.WriteAsync(report.Generate());
}
else
{
    var diagnostics = result.Diagnostics;
}

/**
 *
 */
public static class Demo
{
    public static void Foo()
    {
        var parser = new POParser(new POParserSettings
        {
            // PreserveHeadersOrder = true,
            StringDecodingOptions = new POStringDecodingOptions
            {
                KeepKeyStringsPlatformIndependent = true,
                KeepTranslationStringsPlatformIndependent = true,
            },
        });

        TextReader reader = new StreamReader("Game.po");

        var result = parser.Parse(reader);
        if (result.Success)
        {
            var catalog = result.Catalog;

            var generator = new POGenerator(new POGeneratorSettings
            {
                // PreserveHeadersOrder = true,
            });

            TextWriter writer = new StreamWriter("Test.po");
            generator.Generate(writer, catalog);
            writer.Flush();

            using (var package = new ExcelPackage("Test.xlsx"))
            {
                var file = new FileInfo("Test.csv");
                var format = new ExcelOutputTextFormat()
                {
                    Delimiter = ',',
                    Encoding = Encoding.UTF8,
                };

                if (!package.Workbook.Worksheets.Any())
                {
                    package.Workbook.Worksheets.Add("Default");
                }

                var sheet = package.Workbook.Worksheets[0];
                sheet.Cells["A1"].Value = "Key";
                sheet.Cells["B1"].Value = "Source";
                int i = 0;
                for (; i < catalog.Count; i++)
                {
                    sheet.Cells[i + 2, 1].Value = catalog[i].Key.Id;
                    sheet.Cells[i + 2, 2].Value = catalog.GetTranslation(catalog[i].Key);
                }

                sheet.Cells[1, 1, i + 2, 2].SaveToText(file, format);
                package.Save();
            }
        }
        else
        {
            var diagnostics = result.Diagnostics;
        }
    }
}