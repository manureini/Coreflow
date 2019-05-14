
namespace FlowNs___cc_e4090fcd_cf72_4071_899a_44e9f704297a {
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;

//#id e4090fcd-cf72-4071-899a-44e9f704297a
//#Container
[Coreflow.Objects.FlowIdentifierAttribute("e4090fcd-cf72-4071-899a-44e9f704297a")]
public class Flow___cc_e4090fcd_cf72_4071_899a_44e9f704297a : Coreflow.Interfaces.ICompiledFlow  {

public System.Guid InstanceId {get; set;} = Guid.NewGuid();
public Coreflow.Coreflow CoreflowInstace{ get; set; }
public Coreflow.Interfaces.IArgumentInjectionStore ArgumentInjectionStore{ get; set; }
public Microsoft.Extensions.Logging.ILogger Logger{ get; set; }


public void SetArguments(IDictionary<string, object> pArguments) {
}

public IDictionary<string, object> GetArguments() {
Dictionary<string, object> ret = new Dictionary<string, object>();
return ret;
}


public void Run() { 
//#id 58ec97ae-2ca5-44ac-aa16-0fbd08c6d835
//#Container
{

{
//#id acafbf10-aa4d-4be7-aac2-402fb93dd491
System.Diagnostics.Debugger.Launch();
System.Console.WriteLine("Wait for debugger...");
while (!System.Diagnostics.Debugger.IsAttached) {
System.Threading.Thread.Sleep(500);
}
System.Diagnostics.Debugger.Break();


//#id b5fc53e1-ba6c-4e53-ad3c-be9925444a62
//#Container
{
//#id d31fe672-416e-4082-9781-05671bdfce7f
global::Coreflow.ConsoleWriteLineActivity __cc_d31fe672_416e_4082_9781_05671bdfce7f = new global::Coreflow.ConsoleWriteLineActivity();
//#id 59a09a0f-bae2-4ecb-8ba0-926a9eca58bc
global::Coreflow.SleepActivity __cc_59a09a0f_bae2_4ecb_8ba0_926a9eca58bc = new global::Coreflow.SleepActivity();
while (
//#id 5ef5b0b4-9efe-4042-962c-39f96ab5dbd7
true
)

{
//#id d31fe672-416e-4082-9781-05671bdfce7f
__cc_d31fe672_416e_4082_9781_05671bdfce7f.Execute(

//#id 3b39bf56-6635-4fc0-97fe-342ab06b8288
DateTime.Now.Millisecond + ""
);


//#id 59a09a0f-bae2-4ecb-8ba0-926a9eca58bc
__cc_59a09a0f_bae2_4ecb_8ba0_926a9eca58bc.Execute(

//#id 045dd8cb-cfd4-49fa-ac44-964ba276fa70
1000
);


//#id 383b6d61-4933-43dd-9276-afee8f1052b8
//#id 3911f6af-826e-4f6f-9ca8-b858ac39ad22
System.Diagnostics.Debugger.Break();


}
}


/* !!! --------------------------------------------------------------- !!!*/
}
}
} /* Run */
} /* Class */
} /* Namespace */
