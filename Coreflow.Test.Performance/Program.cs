using BenchmarkDotNet.Running;
using Coreflow.Storage;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Coreflow.Test.Performance
{
    class Program
    {
        public static int count = 5_000_000;
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

            {
                Stopwatch watchSecond = new Stopwatch();

                watchSecond.Start();
                mCoreflow.RunFlow(mIdentifier);
                watchSecond.Stop();

                Console.WriteLine("second single: " + watchSecond.GetNanoseconds() + "ns");
            }

            {
                Stopwatch watchFive = new Stopwatch();

                watchFive.Start();

                for (int i = 0; i < 5; i++)
                    mCoreflow.RunFlow(mIdentifier);

                watchFive.Stop();

                Console.WriteLine("5x start: " + (watchFive.GetNanoseconds() / 5) + "ns");
            }


            {
                Stopwatch watchMany = new Stopwatch();
                watchMany.Start();

                for (int i = 0; i < count; i++)
                {
                    mCoreflow.RunFlow(mIdentifier);
                }

                watchMany.Stop();

                Console.WriteLine("many: " + (watchMany.GetNanoseconds() / count) + "ns");
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

                Console.WriteLine("multithread: " + (watchMultiThread.GetNanoseconds() / (count * threadCount)) + "ns");


            }


        }

        static void Run()
        {
            var factory = mCoreflow.FlowManager.GetFactory(mIdentifier);

            PiCalculator calc = new PiCalculator();

            for (int i = 0; i < count; i++)
            {
                //  factory.RunInstance();

                calc.GetPi(20, 3_000_000);

            }

            Console.WriteLine("thread done");

            mCountdownEvent.Signal();
        }

    }
}
