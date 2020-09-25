using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace TracerLib.Tests
{
    [TestClass]
    public class TracerTests
    {
        static readonly int sleepTime = 15;
        static Tracer tracer;

        [TestInitialize]
        public void TestInit()
        {
            tracer = new Tracer();
        }

        static public class SimpleTest
        {
            static public void SimpleCall()
            {
                tracer.StartTrace();

                Thread.Sleep(sleepTime);

                tracer.StopTrace();
            }
        }

        static public class SequentialTest
        {
            static public void SequentialCall()
            {
                SimpleTest.SimpleCall();
                SimpleTest.SimpleCall();
                SimpleTest.SimpleCall();
            }
        }

        static public class NestedTest
        {
            static public void NestedCall()
            {
                tracer.StartTrace();

                SimpleTest.SimpleCall();

                tracer.StopTrace();
            }
        }

        static public class NestedSequentialTest
        {
            static public void NestedSequentialCall()
            {
                tracer.StartTrace();

                SimpleTest.SimpleCall();
                SimpleTest.SimpleCall();
                SimpleTest.SimpleCall();

                tracer.StopTrace();
            }
        }

        static public class MutualRecursionTest
        {
            static public void MutualCall(int count)
            {
                tracer.StartTrace();

                if (count != 0)
                    MutualCall2(count);
                Thread.Sleep(sleepTime);

                tracer.StopTrace();
            }

            static public void MutualCall2(int count)
            {
                tracer.StartTrace();

                if (count != 0)
                    MutualCall(count - 1);
                Thread.Sleep(sleepTime);

                tracer.StopTrace();
            }
        }

        static public class ThreadTest
        {
            static public void NewThreadCall()
            {
                tracer.StartTrace();

                Thread.Sleep(sleepTime);

                tracer.StopTrace();
            }

            static public void CreateNewThread()
            {
                tracer.StartTrace();

                ThreadStart threadStart = new ThreadStart(NewThreadCall);
                Thread thread = new Thread(threadStart);
                thread.Start();
                thread.Join();

                tracer.StopTrace();
            }
        }

        static void CheckEqual(MethodTraceResult expected, MethodTraceResult result)
        {
            Assert.AreEqual(expected.Class, result.Class);
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.Methods.Count, result.Methods.Count);
            Assert.IsNotNull(result.Time);
        }



        [TestMethod]
        public void MeasureSimpleCall_None_Equal()
        {
            SimpleTest.SimpleCall();
            var actual = tracer.GetTraceResult().Threads[0].Methods[0];
            var expected = new MethodTraceResult("SimpleTest", "SimpleCall");
            CheckEqual(expected, actual);
        }

        [TestMethod]
        public void MeasureSequentialCall_None_Equal()
        {
            SequentialTest.SequentialCall();
            var actual = tracer.GetTraceResult().Threads[0].Methods;
            var expected = new MethodTraceResult("SimpleTest", "SimpleCall");

            foreach(MethodTraceResult m in actual)
                CheckEqual(expected, m);
        }

        [TestMethod]
        public void MeasureNestedCall_None_Equal()
        {
            NestedTest.NestedCall();
            var actual = tracer.GetTraceResult().Threads[0].Methods[0];
            var expected = new MethodTraceResult("NestedTest", "NestedCall");
            expected.Methods.Add(new MethodTraceResult("SimpleTest", "SimpleCall"));

            CheckEqual(expected, actual);
        }

        [TestMethod]
        public void MeasureNestedSequentialCall_None_Equal()
        {
            NestedSequentialTest.NestedSequentialCall();
            var actual = tracer.GetTraceResult().Threads[0].Methods[0];
            var expected = new MethodTraceResult("NestedSequentialTest", "NestedSequentialCall");
            expected.Methods.Add(new MethodTraceResult("SimpleTest", "SimpleCall"));
            expected.Methods.Add(new MethodTraceResult("SimpleTest", "SimpleCall"));
            expected.Methods.Add(new MethodTraceResult("SimpleTest", "SimpleCall"));

            CheckEqual(expected, actual);
        }

        [TestMethod]
        public void MeasureMutualRecursionCall_int_1_Equal()
        {
            MutualRecursionTest.MutualCall(1);
            var actual = tracer.GetTraceResult().Threads[0].Methods[0];
            var expected = new MethodTraceResult("MutualRecursionTest", "MutualCall");
            expected.Methods.Add(new MethodTraceResult("MutualRecursionTest", "MutualCall2"));
            expected.Methods[0].Methods.Add(new MethodTraceResult("MutualRecursionTest", "MutualCall"));

            CheckEqual(expected, actual);
            CheckEqual(expected.Methods[0], actual.Methods[0]);
            CheckEqual(expected.Methods[0].Methods[0], actual.Methods[0].Methods[0]);
        }

        [TestMethod]
        public void MeasureThreadCall_None_Equal()
        {
            ThreadTest.CreateNewThread();
            var actual = tracer.GetTraceResult().Threads;
            var expected = new MethodTraceResult("ThreadTest", "CreateNewThread");
            var expected1 = new MethodTraceResult("ThreadTest", "NewThreadCall");

            Assert.AreEqual(2, actual.Count);
            CheckEqual(expected, actual[1].Methods[0]);
            CheckEqual(expected1, actual[0].Methods[0]);
        }
    }
}
