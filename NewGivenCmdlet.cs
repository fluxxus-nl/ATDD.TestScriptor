using System.Management.Automation;

namespace ATDD.TestScriptor
{
    [Cmdlet(VerbsCommon.New, "Given")]
    [Alias("Given")]
    public class NewGivenCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Situation { get; set; }

        protected override void EndProcessing()
        {
            WriteObject(
                new Given(Situation)
            );
        }
    }
}