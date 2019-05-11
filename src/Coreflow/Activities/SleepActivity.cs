using Coreflow.Interfaces;
using Coreflow.Objects;
using System.Threading;

namespace Coreflow
{
    [DisplayMeta("Sleep", "Basic", "fa-hourglass-half")]
    public class SleepActivity : ICodeActivity
    {
        public void Execute(
            [DisplayMeta("Milliseconds")]
           int pMilliseconds
            )
        {
            Thread.Sleep(pMilliseconds);
        }
    }
}
