using System.Management.Automation;

namespace ATDD.TestScriptor
{
    [Cmdlet(VerbsCommon.New, "ATDDGiven")]
    [Alias("Given")]
    public class NewATDDGivenCmdlet : Cmdlet
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