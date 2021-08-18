using Library.Collections;
using Library.Helpers;

namespace LibraryTest
{
    [TestClass]
    public class FluentListTest
    {
        private FluentList<int> _List;
        private static readonly Func<int, int> SelfIntFunc = x => x;
        private static readonly Action<int> EmptyIntAction = x => { };

        [TestInitialize]
        public void Initialize() => this._List = FluentList<int>.Create(Enumerable.Range(0, 10));

        [TestMethod]
        public void IndexerTest() => Assert.AreEqual(5, this._List[5]);

        [TestMethod]
        public void CountTest() => Assert.AreEqual(10, this._List.Count);

        [TestMethod]
        public void CreateTest()
        {
            _ = FluentList<int>.Create();
            _ = FluentList<int>.Create(this._List);
        }

        [TestMethod]
        public void IndexOfTest()
        {
            var (_, result) = this._List.IndexOf(5);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void InsertTest() => this._List.Insert(5, 5);

        [TestMethod]
        public void RemoveTest() => this._List.Remove(5);

        [TestMethod]
        public void RemoveAtTest() => this._List.RemoveAt(5);

        [TestMethod]
        public void AddTest() => this._List.Add(5);

        [TestMethod]
        public void ClearTest() => this._List.Clear();

        [TestMethod]
        public void ContainsTest() => Assert.IsTrue(this._List.Contains(4).Result);

        [TestMethod]
        public void IterationTest() => this._List.ForEach(EmptyIntAction);
    }
}
