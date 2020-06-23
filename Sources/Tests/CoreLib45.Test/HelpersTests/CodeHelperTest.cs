// Created on     2018/07/25
// Last update on 2018/07/28 by Mohammad Mir mostafa 

using System;
using System.Data;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mohammad.Exceptions;
using static Mohammad.Helpers.CodeHelper;

namespace CoreLib45.Test.HelpersTests
{
    [TestClass]
    public class CodeHelperTest
    {
        [TestMethod]
        public void TestCatch1()
        {
            var res = Catch(() => throw new Exception());
            Assert.IsNotNull(res);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCatch1_1()
        {
            var res = Catch(null);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void TestCatch2()
        {
            var res = Catch(() =>
            {
            });
            Assert.IsNull(res);
        }

        [TestMethod]
        public void TestCatch3()
        {
            var res = CatchFunc(() => throw new Exception(), ex => true);
            Assert.IsTrue(res);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestCatch3_1() => Catch(() => throw new Exception(),
            ex =>
            {
            },
            throwException: true);

        [TestMethod]
        public void TestCatch4()
        {
            var res = CatchFunc(() => true);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCatch5()
        {
            var res = CatchFunc(() => throw new Exception(), true);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCatch6() => Catch(() => throw new Exception());

        [TestMethod]
        public void TestCatch7()
        {
            var res = CatchFunc(() => true,
                finallyAction: () =>
                {
                });
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCatch8()
        {
            var res = CatchSpecExc<NullReferenceException>(() => throw new NullReferenceException());
            Assert.IsNotNull(res);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCatch9() => CatchSpecExc<NullReferenceException>(() => throw new ArgumentException(),
            ex =>
            {
            });

        [TestMethod]
        public void TestCatch10()
        {
            var res = CatchFunc(() => throw new Exception(), ex => false);
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void TestCatch11()
        {
            var res = CatchFunc(() => true, ex => false);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCatch12()
        {
            var res = CatchFunc(() => true);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestCatch14()
        {
            CatchFunc(() => true, out var ex);
            Assert.IsNull(ex);
        }

        [TestMethod]
        public void TestCatch13()
        {
            var ex = Catch(() => throw new Exception());
            Assert.IsNotNull(ex);
        }

        [TestMethod]
        public void TestDoWhile()
        {
            var counter = 0;
            DoWhile(() =>
                {
                },
                () => ++counter < 10);
        }

        [TestMethod]
        public void TestDoWhileFunc()
        {
            var counter = 0;
            var res = DoWhile(() => true, () => ++counter < 10);
            Assert.IsTrue(res.All(r => r));
        }

        [TestMethod]
        public void TestWhile()
        {
            var counter = 0;
            While(() => ++counter < 10,
                () =>
                {
                });
        }

        [TestMethod]
        public void TestWhileFunc()
        {
            var counter = 0;
            var res = While(() => ++counter < 10, () => true);
            Assert.IsTrue(res.All(r => r));
        }

        [TestMethod]
        public void TestRetry1()
        {
            var counter = 0;
            var isSucceed = Retry(() => ++counter == 10, 100).IsSucceed;
            Assert.IsTrue(isSucceed);
        }

        [TestMethod]
        public void TestRetry2()
        {
            var counter = 0;
            var isSucceed = Retry(() => ++counter == 100, 10).IsSucceed;
            Assert.IsFalse(isSucceed);
        }

        [TestMethod]
        public void ComputeTest() => Compute(new[] {1, 2}.ToList);

        [TestMethod]
        public void GetCurrenntMethodTest() => Assert.AreEqual(GetCurrentMethod().Name, "GetCurrenntMethodTest");

        [TestMethod]
        public void GetCallerMethodNameTest() => Assert.AreEqual(GetCallerMethodName(), "GetCallerMethodNameTest");

        [TestMethod]
        public void GetCallerMethodNameTest3() => Assert.AreEqual(GetCallerMethodName(3), "InvokeMethod");

        [TestMethod]
        public void HasExceptionTest()
        {
            Assert.AreEqual(HasException(() =>
                {
                }),
                false);
            Assert.AreEqual(HasException(() => throw new Exception()), true);
        }

        [TestMethod]
        [ExpectedException(typeof(BreakException))]
        public void BreakTest() => Break();

        [TestMethod]
        public void DoAndLockTest()
        {
            void Action()
            {
            }

            void Action1(int _)
            {
            }

            var actions = GetRepeat(Action, 5);
            Do(actions);
            DoFor(Action, 5);
            DoForAsync(Action, 5);
            DoForAsParallel(Action, 5);
            DoFor(Action1, 5);
            Lock(Action);
            LockAndCatch(Action);
        }

        [TestMethod]
        public void GetDelegateTest()
        {
            var action = GetDelegate<string, Func<string, bool>>("IsNullOrEmpty");
            Assert.IsNotNull(action);
        }

        [TestMethod]
        public void IsDesignTimeTest()
        {
            var action = IsDesignTime();
            Assert.IsFalse(action);
        }

        public string FakeMethod() => "Fake";

        [TestMethod]
        public void Invoke1Test()
        {
            var res = Invoke(this, "FakeMethod");
            Assert.AreEqual(res, "Fake");
        }

        [TestMethod]
        public void Invoke2Test()
        {
            var res = Invoke(this, null);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void RetryTest()
        {
            Assert.IsFalse(Retry(() => false, 3).IsSucceed);
            Assert.IsTrue(Retry(() => true, 3).IsSucceed);

            Assert.IsFalse(Retry(_ => false, 3).IsSucceed);
            Assert.IsTrue(Retry(_ => true, 3).IsSucceed);

            Assert.IsFalse(Retry(() => (isOk: false, result: 5), 3).IsSucceed);
        }

        [TestMethod]
        public void ExecOnDebuggerTest() => ExecOnDebugger(() =>
        {
        });

        [TestMethod]
        public void GetRepeatTest()
        {
            var res = GetRepeat(() =>
                {
                },
                3);
            Assert.AreEqual(res.Count(), 3);
        }

        [TestMethod]
        public void RunAndCleanupMemory1Test() => RunAndCleanupMemory(() =>
        {
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RunAndCleanupMemory2Test() => RunAndCleanupMemory(null);

        [TestMethod]
        public void RunAndCleanupMemoryFuncTest() => Assert.AreEqual(RunAndCleanupMemory(() => 5), 5);

        [TestMethod]
        public void GetCallerMethodTest()
        {
            var res = GetCallerMethod(2, true);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Dispose1Test()
        {
            Dispose(() => new DataTable(),
                t =>
                {
                });
            Dispose(() => new DataTable(), t => 5);
            Dispose<DataTable>(t =>
            {
            });
            Dispose<DataTable, int>(t => 5);
        }

        [TestMethod]
        public void IfTest()
        {
            true.If(() =>
            {
            });
            false.If(() =>
            {
            });

            true.IfTrue(() =>
            {
            });
            false.IfFalse(() =>
            {
            });
        }
    }
}