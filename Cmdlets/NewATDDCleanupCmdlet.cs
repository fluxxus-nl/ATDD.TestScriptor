using System.Management.Automation;

namespace ATDD.TestScriptor
{
    [Cmdlet(VerbsCommon.New, "ATDDCleanup")]
    [Alias("Cleanup")]
    public class NewATDDCleanupCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Target { get; set; }

        protected override void EndProcessing()
        {
            WriteObject(
                new Cleanup(Target)
            );
        }
    }
}