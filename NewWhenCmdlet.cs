using System.Management.Automation;

namespace ATDD.TestScriptor
{
    [Cmdlet(VerbsCommon.New, "When")]
    [Alias("When")]
    public class NewWhenCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Condition { get; set; }

        protected override void EndProcessing()
        {
            WriteObject(
                new When(Condition)
            );
        }
    }
}