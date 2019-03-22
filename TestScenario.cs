using System.Collections.ObjectModel;

namespace ATDD.TestScriptor
{
    public class TestScenario
    {
        public TestScenario(int id, string name, params TestScenarioElement[] elements)
        {
            ID = id;
            Name = name;
            Elements.AddRange(elements);
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public Collection<TestScenarioElement> Elements { get; } = new Collection<TestScenarioElement>();

        public override string ToString() => $"[SCENARIO] {ID} {Name}";
    }
}