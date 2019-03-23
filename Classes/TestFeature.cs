using System;
using System.Collections.ObjectModel;

namespace ATDD.TestScriptor
{
    public class TestFeature
    {
        public TestFeature(string name, params TestScenario[] scenarios)
        {
            Name = name;
            Scenarios = new TestScenarios(this);

            Scenarios.AddRange(scenarios);
        }

        public string Name { get; }
        public TestScenarios Scenarios { get; }

        public override string ToString() => $"[FEATURE] {Name}";
    }
}
