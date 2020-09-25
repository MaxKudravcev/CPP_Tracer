using System;
using TracerLib;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Laba_1
{
    class Program
    {
        static Tracer tracer = new Tracer();
        static int count = 0;

        static void Main(string[] args)
        {
            TestAsync();
            TestA();
            TestB();
            TestC();

            TraceResult tr = tracer.GetTraceResult();


            //TraceResultXMLSerializer ser = new TraceResultXMLSerializer();
            JSONSerializer ser = new JSONSerializer();
            byte[] t = ser.Serialize(tr);
            TraceResultOutput.WriteToStream(t, Console.OpenStandardOutput());
            using (FileStream fs = new FileStream("test.txt", FileMode.OpenOrCreate, FileAccess.Write))
                TraceResultOutput.WriteToStream(t, fs);
            
                Console.ReadLine();
        }

        static void TestA()
        {
            tracer.StartTrace();
            int a = 0;
            for (int i = 0; i < 10000000; i++)
                a++;
            TestB();
            tracer.StopTrace();
        }

        static void TestB()
        {
            tracer.StartTrace();
            int a = 0;
            for (int i = 0; i < 1000000; i++)
                a++;
            tracer.StopTrace();
        }

        static void TestC()
        {
            tracer.StartTrace();
            int a = 0;
            for (int i = 0; i < 1000000; i++)
                a++;
            count++;
            if (count != 2)
                TestD();
            tracer.StopTrace();
        }

        static void TestD()
        {
            tracer.StartTrace();
            int a = 0;
            for (int i = 0; i < 1000000; i++)
                a++;
            TestC();
            tracer.StopTrace();
        }

        static void TestE()
        {
            tracer.StartTrace();
            int a = 0;
            for (int i = 0; i < 1000000; i++)
                a++;
            tracer.StopTrace();
        }

        static void TestAsync()
        {
            tracer.StartTrace();
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            ThreadStart threadStart = new ThreadStart(TestE);
            Thread thread = new Thread(threadStart);
            thread.Start();
            thread.Join();
            //Console.WriteLine("Async job end");

            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            tracer.StopTrace();
        }
    }
}
