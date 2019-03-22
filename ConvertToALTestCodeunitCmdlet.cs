using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text.RegularExpressions;

namespace ATDD.TestScriptor
{
    [Cmdlet(VerbsData.ConvertTo, "ALTestCodeunit")]
    public class ConvertToALTestCodeunitCmdlet : Cmdlet
    {
        private List<TestScenario> scenarioCache = new List<TestScenario>();

        [Parameter(Mandatory = true, Position = 0)]
        public int CodeunitID { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string CodeunitName { get; set; }

        [Parameter(ValueFromPipeline = true)]
        [ValidateNotNull()]
        public TestFeature[] Feature { get; set; } = new TestFeature[] { };

        protected override void ProcessRecord() => scenarioCache.AddRange(Feature.SelectMany(f => f.Scenarios));

        protected override void EndProcessing()
        {
            var features =
                scenarioCache
                    .Select(s => s.Feature.ToString())
                    .Distinct();

            var functionNames =
                scenarioCache
                    .SelectMany(s => s.Elements)
                    .Select(e => SanitizeName(e.Value))
                    .Distinct()
                    .OrderBy(f => f);

            using (var stringWriter = new StringWriter())
            {
                using (var writer = new IndentedTextWriter(stringWriter))
                {
                    writer.WriteLine($"codeunit {CodeunitID} \"{CodeunitName}\"");
                    writer.WriteLine("{");
                    writer.Indent++;
                    writer.WriteLines(features.Select(f => $"// {f}"));
                    writer.WriteLine("SubType = Test;");
                    writer.WriteLine();
                    scenarioCache.ForEach(s => WriteALTestFunction(s, writer));
                    functionNames.ForEach(f => WriteDummyFunction(f, writer));
                    writer.Indent--;
                    writer.WriteLine("}");
                }

                WriteObject(stringWriter.ToString());
            }
        }

        protected void WriteALTestFunction(TestScenario scenario, IndentedTextWriter writer)
        {
            writer.WriteLine("[Test]");
            writer.WriteLine($"procedure {SanitizeName(scenario.Name)}()");
            writer.WriteLine($"// {scenario.Feature.ToString()}");
            writer.WriteLine("begin");
            writer.Indent++;
            writer.WriteLine($"// {scenario.ToString()}");
            writer.WriteLine();
            writer.WriteLines(scenario.Elements.OfType<Given>().SelectMany(g => ElementLines(g)));
            writer.WriteLines(scenario.Elements.OfType<When>().SelectMany(w => ElementLines(w)));
            writer.WriteLines(scenario.Elements.OfType<Then>().SelectMany(t => ElementLines(t)));
            writer.WriteLines(scenario.Elements.OfType<Cleanup>().SelectMany(c => ElementLines(c)));
            writer.Indent--;
            writer.WriteLine("end;");
            writer.WriteLine();
        }

        protected IEnumerable<string> ElementLines(TestScenarioElement element)
        {
            yield return $"// {element.ToString()}";
            yield return $"{SanitizeName(element.Value)}();";
            yield return "";
        }

        protected void WriteDummyFunction(string name, IndentedTextWriter writer)
        {
            writer.WriteLine($"local procedure {name}()");
            writer.WriteLine("begin");
            writer.WriteLine("end;");
            writer.WriteLine();
        }

        protected static string SanitizeName(string name) =>
            Regex.Replace(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name), @"\W", @"");
    }
}