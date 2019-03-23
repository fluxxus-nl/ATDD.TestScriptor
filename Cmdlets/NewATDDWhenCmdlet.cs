using System.Management.Automation;

namespace ATDD.TestScriptor
{
    [Cmdlet(VerbsCommon.New, "ATDDWhen")]
    [Alias("When")]
    public class NewATDDWhenCmdlet : Cmdlet
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