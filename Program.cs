using NSwag.CodeGeneration.CSharp;
using System.Text.RegularExpressions;
using NSwag;


class LichessClientGenerator
{
    private static string FixCode(string code)
    {
        Dictionary<string, string> rules = new Dictionary<string, string>(){
            {@"return default\(void\);", "return;"},
            {@"\s+\[.*\]\n\s*public.*PerfType\n\s+{\n\n\s+}", ""},
            {"PerfType3", "PerfType"},
        };

        var result = code;

        foreach (string pattern in rules.Keys)
        {
            // Console.WriteLine("Matches pattern {0} : {1} times", pattern, Regex.Matches(result, pattern).Count());
            result = Regex.Replace(result, pattern, rules[pattern]);
        }
        
        return result;
    }
    
    public static async Task GenerateClient(string swaggerPath, string outputPath)
    {
        var document = await OpenApiDocument.FromFileAsync(swaggerPath);
        string filename = Path.GetFileNameWithoutExtension(outputPath);
        
        var settings = new CSharpClientGeneratorSettings
        {
            ClassName = filename, 
            CSharpGeneratorSettings = 
            {
                Namespace = "Shatranj"
            }
        };

        var generator = new CSharpClientGenerator(document, settings);	
        var code = generator.GenerateFile();

        // Fix issues related to NSWag
        // https://github.com/RicoSuter/NSwag/issues/3912 
        code = FixCode(code);

        await File.WriteAllTextAsync(outputPath, code);

    }

    static async Task Main(string[] args)
    {
        if (args.Length != 2)
            throw new ArgumentException("Expecting 2 arguments: swaggerPath, outputPath");

        var swaggerPath = args[0];
        var outputPath = Path.Combine(Directory.GetCurrentDirectory(), args[1]);

        await GenerateClient(swaggerPath, outputPath);
        Console.WriteLine("C# Client Generation done!");
    }
}