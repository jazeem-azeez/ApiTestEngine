using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TestClassGenerator
{
    internal class Program
    {


        private static void Main(string[] args)
        {
            args = new string[] { @"C:\Users\jazeez\Source\Repos\IHC.Gateway\Source\Itron.Cloud.HybridConnector.Gateway.Proxy\" };
            var tempate = @"c:\users\jazeez\onedrive - itron\documents\visual studio 2017\Projects\TestClassGenerator\TestClassGenerator\template.json";

            DirectoryInfo info = new DirectoryInfo(args[0]);
            var files = info.GetFiles("*.cs", SearchOption.AllDirectories);
            var failure_Logs = new StringBuilder();
            foreach (var item in files)
            {
                try
                {
                    Run(new string[] { item.FullName, item.Name.Replace(".cs", "Tests.cs"), tempate });
                }
                catch (Exception ex)
                {

                    failure_Logs.AppendLine("Failed for -: " + item.FullName);
                    failure_Logs.AppendLine(ex.Message);
                }
            }

            Console.WriteLine("Logs: ");
            Console.WriteLine(failure_Logs.ToString());
            File.WriteAllText("logs.txt",failure_Logs.ToString());

        }

        private static void Run(string[] args)
        {
            var template = new TestGeneratorTemplate();


            var functionSpan = new Dictionary<string, List<string>>();
            var usings = new List<string>();
            string outputfileName, fileSourceCode;

            ParseInputArgments(args, out template, out outputfileName, out fileSourceCode);

            ParseForUsings(usings, fileSourceCode);
            string namespaceName = GetIdentifierPartof("namespace", fileSourceCode);
            string className = GetIdentifierPartof("class", fileSourceCode);
            if (string.IsNullOrEmpty(className))
            {
                className= outputfileName.Substring(outputfileName.LastIndexOf(@"\")+1).Replace(".cs","");
            }
            ParseForFunctions(functionSpan: functionSpan, fileSourceCode: fileSourceCode);
            Console.WriteLine(namespaceName);
            Console.WriteLine(className);
            var Builder = new StringBuilder();
            OutputUsingsStatment(usings, Builder);
            Builder.AppendLine(GenerateOutPutTestClass(namespaceName, className, functionSpan, template));
            File.WriteAllText(outputfileName, Builder.ToString());
            // Console.ReadLine();
        }

        private static void ParseInputArgments(string[] args, out TestGeneratorTemplate template, out string outputfileName, out string fileSourceCode)
        {
            Console.WriteLine("Parsing Input Filename : ");
            string inputFileName = (args.Length > 0) ? args[0] : Console.ReadLine();
            Console.WriteLine("Parsing OutPut Filename : ");
            outputfileName = (args.Length > 1) ? args[1] : Console.ReadLine();
            Console.WriteLine("Parsing Template Filename : ");
            string templatefileName = (args.Length == 3) ? args[2] : Console.ReadLine();

            if (!File.Exists(inputFileName))
            {
                throw new FileNotFoundException(inputFileName);
            }

            template = JsonConvert.DeserializeObject<TestGeneratorTemplate>(File.ReadAllText(templatefileName));

            if (File.Exists(template.FunctionBodySource))
            {
                template.FunctionBodySource = File.ReadAllText(template.FunctionBodySource);
            }
            if (File.Exists(template.ClassBodySource))
            {
                template.ClassBodySource = File.ReadAllText(template.ClassBodySource);
            }
            fileSourceCode = File.ReadAllText(inputFileName);
        }

        #region OutputGnerators



        private static string GenerateOutPutTestClass(string namespaceName, string className, Dictionary<string, List<string>> functionSpan, TestGeneratorTemplate template)
        {
            var sb = new StringBuilder();

            sb.AppendLine(template.ClassDefPostFix);
            var result = template.ClassBodySource.Replace("///<NameSpaceName/>", $" {namespaceName}.Test")
                                    .Replace("///<ClassName/>", $" {className}Tests")
                                    .Replace("///<FunctionBody/>", GenerateTestFuntionPermutiations(functionSpan, template));
            sb.AppendLine(result);
            sb.AppendLine(template.ClassDefPostFix);
            return sb.ToString();
        }

        private static string GenerateTestFuntionPermutiations(Dictionary<string, List<string>> functionSpan, TestGeneratorTemplate template)
        {
            var sb = new StringBuilder();
            foreach (var item in functionSpan)
            {
                foreach (var argumentName in item.Value)
                {
                    sb.AppendLine(template.FunctionDefPrefix);
                    sb.AppendLine($"public void {argumentName} ()");
                    sb.AppendLine(template.FunctionBodySource);
                    sb.AppendLine(template.FunctionDefPostfix);
                }
                sb.AppendLine(template.FunctionDefPostfix);
            }
            return sb.ToString();
        }

        private static void OutputUsingsStatment(List<string> usings, StringBuilder sb)
        {
            foreach (var item in usings)
            {
                sb.AppendLine($"using {item} ; ");
            }
        }

        #endregion OutputGnerators

        #region InputParsers

        private static void ParseForUsings(List<string> usings, string fileSourceCode)
        {
            var result = Regex.Matches(fileSourceCode, @"using\s*([a-zA-Z0-9,\:,\.]*)\s*;").Cast<Match>()
                         .Select(m => m.Groups[1].Captures[0].Value).ToArray();
            foreach (var item in result)
            {
                usings.Add(item);
                Console.WriteLine(item);
            }
        }

        private static string GetIdentifierPartof(string v, string fileSourceCode)
        {
            var result = Regex.Matches(fileSourceCode,
                         $"{v}" + @"\s*([a-zA-Z0-9,\:,\.]*)\s*{").Cast<Match>()
                         .Select(m => m.Groups[1].Captures[0].Value).ToArray();
            return result.FirstOrDefault();
        }

        private static void ParseForFunctions(Dictionary<string, List<string>> functionSpan, string fileSourceCode)
        {
            if (functionSpan != null)
            {
                var functionNamesCollection = Regex.Matches(fileSourceCode, @"([a-zA-Z0-9]*)\s*\([^()]*\)\s*{").Cast<Match>();

                foreach (var item in functionNamesCollection)
                {
                    var functionLine = item.Value.Trim('{').Trim();
                    ///
                    /// Get function name
                    ///
                    var functionName = item.Groups[1].Captures[0].Value;
                    functionSpan.Add(functionLine, new List<string>());
                    functionSpan[functionLine].Add($"Test_{functionName }");
                    var func = Regex.Match(functionLine, @"\b[^()]+\((.*)\)$");

                    Console.WriteLine("FuncTag: " + func.Value);
                    string innerArgs = func.Groups[1].Value;
                    ///
                    /// Get parameters
                    ///
                    var paramTags = Regex.Matches(innerArgs, @"([^,]+\(.+?\))|([^,]+)");

                    Console.WriteLine("Matches: " + paramTags.Count);
                    foreach (var param in paramTags)
                    {
                        var paramIdentifier = param.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                        Console.WriteLine("ParamTag: " + paramIdentifier);
                        functionSpan[functionLine].Add($"Test_{functionName }{paramIdentifier }");
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("functionSpan");
            }
        }
    }

    #endregion InputParsers
}