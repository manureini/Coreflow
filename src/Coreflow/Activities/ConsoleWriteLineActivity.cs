using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.ComponentModel;

namespace Coreflow
{
    [DisplayMeta("Console.WriteLine", "fa-terminal")]
    public class ConsoleWriteLineActivity : ICodeActivity
    {
        public void Execute(
            [DisplayMeta("Text")]
            string pText
            )
        {
            Console.WriteLine(pText);
        }
    }
}
