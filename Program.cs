// See https://aka.ms/new-console-template for more information

using Karambolo.PO;

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

    var generator = new POGenerator(new POGeneratorSettings
    {
        // PreserveHeadersOrder = true,
    });

    TextWriter writer = new StreamWriter("Test.po");
    generator.Generate(writer, catalog);
    writer.Flush();
}
else
{
    var diagnostics = result.Diagnostics;
}