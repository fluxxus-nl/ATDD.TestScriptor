namespace ATDD.TestScriptor
{
    public class Then : TestScenarioElement
    {
        public Then(string expectedResult) => ExpectedResult = expectedResult;
        public string ExpectedResult { get; }

        public override string ToString() => $"[THEN] {ExpectedResult}";
    }
}
