using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text.RegularExpressions;

namespace ATDD.TestScriptor
{
    [Cmdlet(VerbsData.Sync, "ALTestCodeunit")]
    public class SyncALTestCodeunitCmdlet : PSCmdlet
    {
        private List<TestScenario> scenarioCache = new List<TestScenario>();

        [Parameter(Mandatory = true)]
        public string CodeunitPath { get; set; }

        private SwitchParameter InitializeFunction { get; set; }

        [Parameter(ValueFromPipeline = true)]
        [ValidateNotNull()]
        public TestFeature[] Feature { get; set; } = new TestFeature[] { };

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        public string GivenFunctionName { get; set; } = "{0}";

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        public string WhenFunctionName { get; set; } = "{0}";

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        public string ThenFunctionName { get; set; } = "{0}";

        [Parameter()]
        [ValidateNotNull()]
        public string BannerFormat { get; set; } = "// Generated on {0} at {1} by {2}";

        protected override void ProcessRecord() => scenarioCache.AddRange(Feature.SelectMany(f => f.Scenarios));

        protected override void EndProcessing()
        {
            var uniqueFeatureNames =
                scenarioCache
                    .Select(s => s.Feature.ToString())
                    .Distinct();

            var elementFunctionNames =
                scenarioCache
                    .SelectMany(s => s.Elements)
                    .Select(e => new { Element = e, FunctionName = GetElementFunctionName(e) })
                    .ToDictionary(o => o.Element, o => o.FunctionName);

            var uniqueFunctionNames =
                elementFunctionNames
                    .Values
                    .Distinct()
                    .OrderBy(f => f);


            CodeunitPath = File.Exists(CodeunitPath) ? CodeunitPath : Path.Combine(this.SessionState.Path.CurrentFileSystemLocation.Path, CodeunitPath);
            if (!File.Exists(CodeunitPath))
            {
                return;
            }

            var lines = File.ReadAllLines(CodeunitPath).ToList();
            var uniqueProcedures = scenarioCache.Select(s => s.ToString()).Union(elementFunctionNames.Select(s => s.ToString()));
            var existingFunctions = uniqueFunctionNames.Where(w => lines.Any(a => a.Contains(w.ToString())));
            var newFunctions = uniqueProcedures.Except(existingFunctions);
            var existingScenarios = scenarioCache.Where(s => lines.Any(a => a.Contains(s.ToString())));
            var newScenarios = scenarioCache.Except(existingScenarios);

            var newElementFunctionNames =
                newScenarios
                    .SelectMany(s => s.Elements)
                    .Select(e => new { Element = e, FunctionName = GetElementFunctionName(e) })
                    .ToDictionary(o => o.Element, o => o.FunctionName);

            var newUniqueFunctionNames =
                newElementFunctionNames
                    .Values
                    .Distinct()
                    .OrderBy(f => f);

            newScenarios.ForEach(e => WriteObject($"New Scenario: {e.ToString()}"));
            newUniqueFunctionNames.ForEach(e => WriteObject($"New helper: {e.ToString()}"));

            if (newScenarios.Count() == 0)
            {
                return;
            }

            WarnIfPlaceHolderMissing(GivenFunctionName);
            WarnIfPlaceHolderMissing(WhenFunctionName);
            WarnIfPlaceHolderMissing(ThenFunctionName);

            // scenarios
            using (var stringWriter = new StringWriter())
            {
                using (var writer = new IndentedTextWriter(stringWriter))
                {
                    writer.Indent++;
                    writer.WriteLine();
                    newScenarios.ForEach(s => WriteALTestFunction(s, newElementFunctionNames, writer));
                    writer.Indent--;
                }

                var ti = lines.FindLastIndex(f => f.Contains("// [SCENARIO"));
                var li = lines.FindIndex(ti, f => f.Contains("var"));

                lines.Insert(li, stringWriter.ToString());
            }

            // helpers
            using (var stringWriter = new StringWriter())
            {
                using (var writer = new IndentedTextWriter(stringWriter))
                {
                    writer.Indent++;
                    writer.WriteLine();
                    newUniqueFunctionNames.ForEach(f => WriteDummyFunction(f, writer));
                    writer.Indent--;
                }

                var ti = lines.LastIndexOf("}");

                lines.Insert(ti, stringWriter.ToString());
            }

            //WriteObject(lines.Join("\r\n"));

            File.WriteAllLines(CodeunitPath, lines);
        }

        protected void WriteALTestFunction(TestScenario scenario, Dictionary<TestScenarioElement, string> elementFunctionNames, IndentedTextWriter writer)
        {
            writer.WriteLine("[Test]");
            writer.WriteLine($"procedure {SanitizeName(scenario.Name)}()");
            writer.WriteLine($"// {scenario.Feature.ToString()}");
            writer.WriteLine("begin");
            writer.Indent++;
            writer.WriteLine($"// {scenario.ToString()}");
            writer.WriteLineIf(InitializeFunction, "Initialize();");
            writer.WriteLine();
            writer.WriteLines(scenario.Elements.OfType<Given>().SelectMany(g => ElementLines(g, elementFunctionNames)));
            writer.WriteLines(scenario.Elements.OfType<When>().SelectMany(w => ElementLines(w, elementFunctionNames)));
            writer.WriteLines(scenario.Elements.OfType<Then>().SelectMany(t => ElementLines(t, elementFunctionNames)));
            writer.WriteLines(scenario.Elements.OfType<Cleanup>().SelectMany(c => ElementLines(c, elementFunctionNames)));
            writer.Indent--;
            writer.WriteLine("end;");
            writer.WriteLine();
        }

        protected IEnumerable<string> ElementLines(TestScenarioElement element, Dictionary<TestScenarioElement, string> elementFunctionNames)
        {
            yield return $"// {element.ToString()}";
            yield return $"{elementFunctionNames[element]}();";
            yield return "";
        }

        protected void WriteDummyFunction(string name, IndentedTextWriter writer)
        {
            writer.WriteLine($"local procedure {name}()");
            writer.WriteLine("begin");
            writer.WriteLine("end;");
            writer.WriteLine();
        }

        protected string GetElementFunctionName(TestScenarioElement element)
        {
            switch (element)
            {
                case Given given: return FormatElement(element, GivenFunctionName);
                case When @when: return FormatElement(element, WhenFunctionName);
                case Then then: return FormatElement(element, ThenFunctionName);
                default: return SanitizeName(element.Value);
            }
        }

        protected string FormatElement(TestScenarioElement element, string format)
        {
            try
            {
                return SanitizeName(string.Format(format, element.Value));
            }
            catch (FormatException e)
            {
                throw new FormatException($"Function name format '{format}' should not contain placeholders other than '{{0}}'", e);
            }
        }

        protected void WarnIfPlaceHolderMissing(string format)
        {
            if (!format.Contains("{0}"))
                WriteWarning($"Function name format '{format}' does not contain placeholder '{{0}}'");
        }

        protected static string SanitizeName(string name) =>
            Regex
                .Split(name, @"\W", RegexOptions.CultureInvariant)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => Regex.Replace(s, "^.", m => m.Value.ToUpperInvariant()))
                .Join("");
    }
}