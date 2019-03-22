using System.Collections.ObjectModel;

namespace ATDD.TestScriptor
{
    public class TestScenarios : Collection<TestScenario>
    {
        public TestScenarios(TestFeature feature) => Feature = feature;

        public TestFeature Feature { get; }

        protected override void InsertItem(int index, TestScenario item)
        {
            item.Feature = Feature;
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            this[index].Feature = null;
            base.RemoveItem(index);
        }
    }
}