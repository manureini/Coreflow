using Coreflow.Helper;
using System.Collections.Generic;
using System.Threading;

namespace Coreflow
{
    class Program
    {
        public static void Main(string[] args)
        {
          
            Dictionary<string, object> arguments = new Dictionary<string, object>()
            {
                {"pInput", "testinput!!!!!!!!!!!!!!!!!!!!!" }
            };

            //WorkflowInvoker.Invoke(wf, arguments);

            Thread.Sleep(3000);
        }
    }
}
