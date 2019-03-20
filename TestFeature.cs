using System;
using System.Collections.ObjectModel;

namespace ATDD.TestScriptor
{
    public class TestFeature
    {
        public TestFeature(string name, params TestScenario[] scenarios)
        {
            Name = name;
            Scenarios.AddRange(scenarios);
        }

        public string Name { get; }
        public Collection<TestScenario> Scenarios { get; } = new Collection<TestScenario>();
    }
}
