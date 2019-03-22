namespace ATDD.TestScriptor
{
    public class Given : TestScenarioElement
    {
        public Given(string situation) => Situation = situation;
        public string Situation { get; }

        public override string ToString() => $"[GIVEN] {Situation}";
    }
}