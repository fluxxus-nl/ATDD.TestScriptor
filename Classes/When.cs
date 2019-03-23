namespace ATDD.TestScriptor
{
    public class When : TestScenarioElement
    {
        public When(string condition) => Condition = condition;
        public string Condition { get; }

        public override string ToString() => $"[WHEN] {Condition}";

        public override string Value => Condition;
    }
}