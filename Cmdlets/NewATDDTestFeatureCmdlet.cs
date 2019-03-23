using System.Linq;
using System.Management.Automation;

namespace ATDD.TestScriptor
{
    [Cmdlet(VerbsCommon.New, "ATDDTestFeature")]
    [Alias("Feature")]
    public class NewATDDTestFeatureCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Name { get; set; }

        [Parameter(Position = 1)]
        [ValidateNotNull()]
        public ScriptBlock Scenarios { get; set; } = ScriptBlock.Create("");

        protected override void EndProcessing()
        {
            WriteObject(
                new TestFeature(
                    Name,
                    Scenarios.Invoke().Select(o => o.BaseObject).Cast<TestScenario>().ToArray()
                )
            );
        }
    }
}
