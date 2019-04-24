using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.ComponentModel;
using System.Threading;

namespace Coreflow
{
    [DisplayMeta("Sleep", "fa-hourglass-half")]
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
