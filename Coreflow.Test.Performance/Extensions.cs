using System.Diagnostics;

namespace Coreflow.Test.Performance
{
    public static class Extensions
    {

        public static double GetNanoseconds(this Stopwatch pStopwatch)
        {
            return 1000000000.0 * (double)pStopwatch.ElapsedTicks / Stopwatch.Frequency;
        }

    }
}
