using System.Linq;
using System.Management.Automation;

namespace ATDD.TestScriptor
{
    [Cmdlet(VerbsData.ConvertTo, "ALTestCodeunit")]
    public class ConvertToALTestCodeunitCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public int CodeunitID { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string CodeunitName { get; set; }

        [Parameter(ValueFromPipeline = true)]
        [ValidateNotNull()]
        public TestFeature[] Feature { get; set; } = new TestFeature[] { };

        protected override void ProcessRecord()
        {
            WriteObject(Feature.Select(f => f.ConvertToALTestCodeunit(CodeunitID, CodeunitName)));
        }
    }
}