using Karambolo.PO;
using OfficeOpenXml;

namespace UnrealL10NAutomationTool.StringTableGenerator;

public class Generator(POCatalog inPO, ExcelPackage outTable)
{
    public void Generate()
    {
        var duplicatedPo = inPO.Values
            .GroupBy(x => x.Key.Id)
            .Where(y => y.Count() > 1)
            .ToList();

        var sheets = outTable.Workbook.Worksheets;
        if (!sheets.Any())
        {
            sheets.Add("Default");
        }

        var sheet = sheets[0];
        sheet.ClearFormulas();
        sheet.Cells["A1"].Value = "Key";
        sheet.Cells["B1"].Value = "SourceString";

        int i = 0;
        for (; i < duplicatedPo.Count; i++)
        {
            sheet.Cells[i + 2, 1].Value = duplicatedPo[i].Key;
            sheet.Cells[i + 2, 2].Value = inPO.GetTranslation(duplicatedPo[i].First().Key);
        }
        
        outTable.Save();
    }
}