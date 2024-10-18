using System;
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

        [Parameter()]
        public SwitchParameter InitializeFunction { get; set; }

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
        public SwitchParameter DoNotAddErrorToHelperFunctions { get; set; }

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

            WarnIfPlaceHolderMissing(GivenFunctionName);
            WarnIfPlaceHolderMissing(WhenFunctionName);
            WarnIfPlaceHolderMissing(ThenFunctionName);

            using (var stringWriter = new StringWriter())
            {
                using (var writer = new IndentedTextWriter(stringWriter))
                {
                    writer.WriteLine($"codeunit {CodeunitID} \"{CodeunitName}\"");
                    writer.WriteLine("{");
                    writer.Indent++;
                    WriteBanner(writer);
                    writer.WriteLine("Access = Public;");
                    writer.WriteLine("InherentEntitlements = X;");
                    writer.WriteLine("InherentPermissions = X;");
                    writer.WriteLine("Subtype = Test;");
                    writer.WriteLine();
                    writer.WriteLine("trigger OnRun()");
                    writer.WriteLine("begin");
                    writer.Indent++;
                    writer.WriteLines(uniqueFeatureNames.Select(f => $"// {f}"));
                    writer.Indent--;
                    writer.WriteLine("end;");
                    writer.WriteLine();
                    scenarioCache.ForEach(s => WriteALTestFunction(s, elementFunctionNames, writer));
                    WriteInitializeFunction(writer);
                    uniqueFunctionNames.ForEach(f => WriteDummyFunction(f, writer));
                    writer.Indent--;
                    writer.WriteLine("}");
                }

                WriteObject(stringWriter.ToString());
            }
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

        protected void WriteBanner(IndentedTextWriter writer)
        {
            var now = DateTime.Now;
            var banner =
                string.Format(
                    BannerFormat,
                    now.ToShortDateString(),
                    now.ToShortTimeString(),
                    Environment.UserName);

            if (!string.IsNullOrEmpty(banner))
            {
                writer.WriteLine(banner);
                writer.WriteLine();
            }
        }

        protected void WriteInitializeFunction(IndentedTextWriter writer)
        {
            if (InitializeFunction)
            {
                writer.WriteLine("var");
                writer.Indent++;
                writer.WriteLine("IsInitialized: Boolean;");
                writer.Indent--;
                writer.WriteLine();

                writer.WriteLine("local procedure Initialize()");
                writer.WriteLine("var");
                writer.Indent++;
                writer.WriteLine("LibraryTestInitialize: Codeunit \"Library - Test Initialize\";");
                writer.Indent--;
                writer.WriteLine("begin");
                writer.Indent++;
                writer.WriteLine($"LibraryTestInitialize.OnTestInitialize(Codeunit::\"{CodeunitName}\");");
                writer.WriteLine();
                writer.WriteLine("if IsInitialized then");
                writer.Indent++;
                writer.WriteLine("exit;");
                writer.Indent--;
                writer.WriteLine();
                writer.WriteLine($"LibraryTestInitialize.OnBeforeTestSuiteInitialize(Codeunit::\"{CodeunitName}\");");
                writer.WriteLine();
                writer.WriteLine("IsInitialized := true;");
                writer.WriteLine("Commit();");
                writer.WriteLine();
                writer.WriteLine($"LibraryTestInitialize.OnAfterTestSuiteInitialize(Codeunit::\"{CodeunitName}\");");
                writer.Indent--;
                writer.WriteLine("end;");
                writer.WriteLine();
            }
        }

        protected void WriteDummyFunction(string name, IndentedTextWriter writer)
        {
            writer.WriteLine($"local procedure {name}()");
            writer.WriteLine("begin");
            if (!DoNotAddErrorToHelperFunctions)
            {
                writer.Indent++;
                writer.WriteLine($"Error('{name} not implemented.')");
                writer.Indent--;
            }
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