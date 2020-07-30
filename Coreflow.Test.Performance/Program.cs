using Coreflow.Runtime.Storage;
using Coreflow.Storage;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Coreflow.Test.Performance
{
    class Program
    {
        public static int count = 100_000;
        public static Coreflow mCoreflow;
        public static Guid mIdentifier;
        public static CountdownEvent mCountdownEvent;

        static void Main(string[] args)
        {
            Stopwatch watchInit = new Stopwatch();
            watchInit.Start();

            var dstorage = new SimpleFlowDefinitionFileStorage(@"Flows");
            var istorage = new NoFlowInstanceStorage();

            mCoreflow = new Coreflow(dstorage, istorage, null);

            var def = mCoreflow.FlowDefinitionStorage.GetDefinitions().First(d => d.Name == "empty");

            mIdentifier = def.Identifier;

            mCoreflow.CompileFlows();

            watchInit.Stop();

            Console.WriteLine("init: " + watchInit.GetNanoseconds() + "ns");


            {
                Stopwatch watchFirst = new Stopwatch();

                watchFirst.Start();
                mCoreflow.RunFlow(mIdentifier);
                watchFirst.Stop();

                Console.WriteLine("first: " + watchFirst.GetNanoseconds() + "ns");
            }

            File.Delete("out");

            {
                Stopwatch watchSecond = new Stopwatch();

                for (int j = 1; j < 100; j++)
                {
                    watchSecond.Reset();
                    watchSecond.Start();
                    for (int i = 0; i < j; i++)
                        mCoreflow.RunFlow(mIdentifier);
                    watchSecond.Stop();

                    string text = j + ":  " + watchSecond.GetNanoseconds() + "ns";
                    File.AppendAllText("out", Environment.NewLine + text);
                    Console.WriteLine(text);
                }
            }


            {
                Stopwatch watchMany = new Stopwatch();
                watchMany.Start();

                for (int i = 0; i < count; i++)
                {
                    mCoreflow.RunFlow(mIdentifier);
                }

                watchMany.Stop();

                Console.WriteLine("many: " + (watchMany.GetNanoseconds()) + "ns");
            }


            {
                Stopwatch watchMultiThread = new Stopwatch();
                watchMultiThread.Start();

                int threadCount = Environment.ProcessorCount;

                mCountdownEvent = new CountdownEvent(threadCount);
                Thread[] threads = new Thread[threadCount];

                for (int i = 0; i < threadCount; i++)
                {
                    threads[i] = new Thread(Run);
                    threads[i].Priority = ThreadPriority.Highest;
                }

                for (int i = 0; i < threadCount; i++)
                    threads[i].Start();

                mCountdownEvent.Wait();

                watchMultiThread.Stop();

                Console.WriteLine("multithread: " + (watchMultiThread.GetNanoseconds()) + "ns");
            }


        }

        static void Run()
        {
            var factory = mCoreflow.FlowManager.GetFactory(mIdentifier);


            for (int i = 0; i < 100_000; i++)
            {
                factory.RunInstance();

                //  calc.GetPi(20, 3_000_000);

            }

            Console.WriteLine("thread done");

            mCountdownEvent.Signal();
        }

    }
}
