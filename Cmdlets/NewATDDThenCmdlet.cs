using System.Management.Automation;

namespace ATDD.TestScriptor
{
    [Cmdlet(VerbsCommon.New, "ATDDThen")]
    [Alias("Then")]
    public class NewATDDThenCmdlet : Cmdlet
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