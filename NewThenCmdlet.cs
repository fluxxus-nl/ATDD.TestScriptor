using System.Management.Automation;

namespace ATDD.TestScriptor
{
    [Cmdlet(VerbsCommon.New, "Then")]
    [Alias("Then")]
    public class NewThenCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string ExpectedResult { get; set; }

        protected override void EndProcessing()
        {
            WriteObject(
                new Then(ExpectedResult)
            );
        }
    }
}