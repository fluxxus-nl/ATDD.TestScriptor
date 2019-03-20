using System.Linq;
using System.Management.Automation;

namespace ATDD.TestScriptor
{
    [Cmdlet(VerbsCommon.New, "TestScenario")]
    [Alias("Scenario")]
    public class NewTestScenarioCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public int ID { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string Name { get; set; }

        [Parameter(Position = 2)]
        [ValidateNotNull()]
        public ScriptBlock Elements { get; set; } = ScriptBlock.Create("");

        protected override void EndProcessing()
        {
            var elements = Elements.Invoke().Select(o => o.BaseObject).Cast<TestScenarioElement>();

            if (!elements.OfType<Given>().Any())
                WriteWarning($"Scenario {ID} {Name} does not contain any elements of type Given.");
            if (!elements.OfType<When>().Any())
                WriteWarning($"Scenario {ID} {Name} does not contain any elements of type When.");
            if (!elements.OfType<Then>().Any())
                WriteWarning($"Scenario {ID} {Name} does not contain any elements of type Then.");

            WriteObject(
                new TestScenario(
                    ID,
                    Name,
                    elements.ToArray())
            );
        }
    }
}