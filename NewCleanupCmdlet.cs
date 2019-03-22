using System.Management.Automation;

namespace ATDD.TestScriptor
{
    [Cmdlet(VerbsCommon.New, "Cleanup")]
    [Alias("Cleanup")]
    public class NewCleanupCmdlet : Cmdlet
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