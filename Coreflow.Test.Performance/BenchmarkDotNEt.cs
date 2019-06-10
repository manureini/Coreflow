using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Test.Performance
{

    [ClrJob(baseline: true), CoreJob, MonoJob, CoreRtJob]
    [RPlotExporter, RankColumn]
    public class Md5VsSha256
    {

        [Params(1000, 10000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {

        }

        [Benchmark]
        public void CalcPi()
        {

            //   var factory = Program.mCoreflow.FlowManager.GetFactory(Program.mIdentifier);

            PiCalculator calc = new PiCalculator();


            //  factory.RunInstance();

            calc.GetPi(20, 10_000);




        }
    }



}
