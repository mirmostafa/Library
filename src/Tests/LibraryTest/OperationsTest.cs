using Library.MultiOperation;

namespace UnitTest;

[TestClass]
public class OperationsTest
{
    [TestMethod]
    public void MyTestMethod()
    {
        var display = (int state, int index, int count) => Console.WriteLine($"state: {state}, index: {index}, count: {count}");
        var result = Operations.New(5)
                               .Add((state) => 10)
                               .Add((state, index) => 20)
                               .Add((state, index, count) => 30)
                               //.AsSequenctial()
                               .Build()
                               .Watch(display)
                               .Run();
    }

    [TestMethod]
    public void MyTestMethod1()
    {
        var r = Operations.New(Task.FromResult(1))
                          .Add(state => Task.FromResult(2))
                          .Add(state => Task.FromResult(3))
                          .Add(state => Task.FromResult(4))
                          .Add(state => Task.FromResult(5))
                          .Build()
                          .Watch((state, index, count) => Console.WriteLine(state.Id))
                          .Run();
        r.Wait();
    }
}