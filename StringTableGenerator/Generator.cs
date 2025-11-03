using Karambolo.PO;
using OfficeOpenXml;

namespace UnrealL10NAutomationTool.StringTableGenerator;

public class Generator(POCatalog inPo, ExcelPackage outTable)
{
    public Report Generate()
    {
        Report report = new()
        {
            Summary = "Err when calculating summary",
            Items = [],
        };
        var duplicatedPo = inPo.Values
            .GroupBy(x => x.Key.Id)
            .Where(y => y.Count() > 1)
            .ToList();

        var sheets = outTable.Workbook.Worksheets;
        if (sheets.Count == 0)
        {
            sheets.Add("Default");
        }

        var sheet = sheets[0];
        sheet.ClearFormulas();
        sheet.Cells["A1"].Value = "Key";
        sheet.Cells["B1"].Value = "SourceString";

        var i = 0;
        for (; i < duplicatedPo.Count; i++)
        {
            var key = duplicatedPo[i].Key;
            var sourceString = inPo.GetTranslation(duplicatedPo[i].First().Key);
            sheet.Cells[i + 2, 1].Value = key;
            sheet.Cells[i + 2, 2].Value = sourceString;
            report.Items.Add(new ReportItem
            {
                Key = key,
                SourceString = sourceString,
                Sources = duplicatedPo[i].Select(entry => entry.Comments[2].ToString() ?? "").ToList(),
            });
        }
        
        outTable.Save();
        report.Num = duplicatedPo.Count;
        report.Summary = $"Generated {report.Num} row(s)";

        return report;
    }
}