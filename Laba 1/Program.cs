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


            Console.WriteLine("Which format do you want to serialize result to:");
            Console.WriteLine("1) XML");
            Console.WriteLine("2) JSON");
            ISerializer ser;
            string inp = Console.ReadLine();
            if(inp == "1")
                ser = new TraceResultXMLSerializer();
            else
                ser = new JSONSerializer();

            byte[] t = ser.Serialize(tr);
            Console.WriteLine();
            Console.WriteLine("Possible options to output the resutlt:");
            Console.WriteLine("1) Console");
            Console.WriteLine("2) .txt file");
            inp = Console.ReadLine();
            if(inp == "1")
                TraceResultOutput.WriteToStream(t, Console.OpenStandardOutput());
            else
                using (FileStream fs = new FileStream("test.txt", FileMode.OpenOrCreate, FileAccess.Write))
                    TraceResultOutput.WriteToStream(t, fs);

            Console.WriteLine("\nThe result has been outputted successfully. Press any key to exit the program.");
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
           

            ThreadStart threadStart = new ThreadStart(TestE);
            Thread thread = new Thread(threadStart);
            thread.Start();
            thread.Join();
            

            
            tracer.StopTrace();
        }
    }
}
