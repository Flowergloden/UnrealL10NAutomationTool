namespace UnrealL10NAutomationTool.StringTableGenerator;

public static class ExtendedReport
{
    public static string Generate(this Report report)
    {
        return string.Join('\n',
            new List<string>([report.Summary])
                .Concat(report.Items
                    .SelectMany(item => new List<string>([$"({item.Key}, {item.SourceString})"])
                        .Concat(item.Sources.Select(source => '\t' + source))))
        );
    }
}

public struct Report
{
    public int Num;
    public string Summary;
    public List<ReportItem> Items;
}

public struct ReportItem
{
    public string Key;
    public string SourceString;
    public List<string> Sources;
}