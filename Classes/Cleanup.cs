namespace ATDD.TestScriptor
{
    public class Cleanup : TestScenarioElement
    {
        public Cleanup(string target) => Target = target;

        public string Target { get; }

        public override string ToString() => $"[CLEANUP] {Target}";

        public override string Value => Target;
    }
}