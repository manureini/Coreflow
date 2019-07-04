using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.IO;

namespace Coreflow.Activities.OrangePi
{
    [DisplayMeta("Set Led Status", "OrangePi", "fa-lightbulb")]
    public class SetLedStatus : ICodeActivity
    {
        public void Execute(string LedName, bool Status)
        {
            string rootPath = $"/sys/class/leds/{LedName}/";
            File.WriteAllText(rootPath + "trigger", "none");
            File.WriteAllText(rootPath + "brightness", Status ? "1" : "0");
        }
    }
}
