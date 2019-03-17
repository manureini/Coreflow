using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.ComponentModel;

namespace Coreflow
{
    [UiDesign("Console.WriteLine", "fa-terminal")]
    public class ConsoleWriteLineActivity : ICodeActivity
    {
        public Guid InstanceId;

        public void Execute(
            [UiDesign("Text")]
            string pText
            )
        {
            Console.WriteLine(pText);
        }
    }
}
