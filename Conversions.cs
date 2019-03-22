using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ATDD.TestScriptor
{
    public static class Conversions
    {
        public static string ConvertToALTestCodeunit(this TestFeature feature, int codeunitID, string codeunitName)
        {
            using (var stringWriter = new StringWriter())
            {
                feature.WriteALTestCodeunit(codeunitID, codeunitName, stringWriter);
                return stringWriter.ToString();
            }
        }

        public static void WriteALTestCodeunit(this TestFeature feature, int codeunitID, string codeunitName, TextWriter textWriter)
        {
            using (var writer = new IndentedTextWriter(textWriter))
            {
                writer.WriteLine($"codeunit {codeunitID} \"{codeunitName}\"");
                writer.WriteLine("{");
                writer.Indent++;
                writer.WriteLine($"// [FEATURE] {feature.Name}");
                writer.WriteLine("SubType = Test;");
                writer.WriteLine();
                feature.Scenarios.ForEach(s => s.WriteALTestFunction(feature, writer));
                writer.Indent--;
                writer.WriteLine("}");
            }
        }

        public static void WriteALTestFunction(this TestScenario scenario, TestFeature feature, IndentedTextWriter writer)
        {
            writer.WriteLine("[Test]");
            writer.WriteLine($"procedure {scenario.ALTestFunctionName()}()");
            writer.WriteLine($"// [FEATURE] {feature.Name}");
            writer.WriteLine("begin");
            writer.Indent++;
            writer.WriteLine($"// [SCENARIO #{scenario.ID} {scenario.Name}]");
            writer.WriteLine();
            writer.WriteLines(scenario.Elements.OfType<Given>().Select(g => $"// [GIVEN] {g.Situation}"));
            writer.WriteLines(scenario.Elements.OfType<When>().Select(w => $"// [WHEN] {w.Condition}"));
            writer.WriteLines(scenario.Elements.OfType<Then>().Select(t => $"// [THEN] {t.ExpectedResult}"));
            writer.WriteLines(scenario.Elements.OfType<Cleanup>().Select(c => $"// [CLEANUP] {c.Target}"));
            writer.Indent--;
            writer.WriteLine("end;");
            writer.WriteLine();
        }

        public static string ALTestFunctionName(this TestScenario scenario) =>
            Regex.Replace(scenario.Name, @"\W", @"");
    }
}