using System.CodeDom.Compiler;
using System.Collections.Generic;
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
            var features = scenarioCache.Select(s => s.Feature.ToString()).Distinct();

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
                    writer.Indent--;
                    writer.WriteLine("}");
                }

                WriteObject(stringWriter.ToString());
            }
        }

        public void WriteALTestFunction(TestScenario scenario, IndentedTextWriter writer)
        {
            writer.WriteLine("[Test]");
            writer.WriteLine($"procedure {ALTestFunctionName(scenario)}()");
            writer.WriteLine($"// {scenario.Feature.ToString()}");
            writer.WriteLine("begin");
            writer.Indent++;
            writer.WriteLine($"// {scenario.ToString()}");
            writer.WriteLine();
            writer.WriteLines(scenario.Elements.OfType<Given>().Select(g => $"// {g.ToString()}"));
            writer.WriteLines(scenario.Elements.OfType<When>().Select(w => $"// {w.ToString()}"));
            writer.WriteLines(scenario.Elements.OfType<Then>().Select(t => $"// {t.ToString()}"));
            writer.WriteLines(scenario.Elements.OfType<Cleanup>().Select(c => $"// {c.ToString()}"));
            writer.Indent--;
            writer.WriteLine("end;");
            writer.WriteLine();
        }

        public static string ALTestFunctionName(TestScenario scenario) =>
            Regex.Replace(scenario.Name, @"\W", @"");
    }
}