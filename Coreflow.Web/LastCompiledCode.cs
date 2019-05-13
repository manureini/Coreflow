
namespace FlowNs___cc_a1bfd8b5_d453_4fe0_9825_59cd67f02d5e {
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;

//#id a1bfd8b5-d453-4fe0-9825-59cd67f02d5e
//#Container
[Coreflow.Objects.FlowIdentifierAttribute("a1bfd8b5-d453-4fe0-9825-59cd67f02d5e")]
public class Flow___cc_a1bfd8b5_d453_4fe0_9825_59cd67f02d5e : Coreflow.Interfaces.ICompiledFlow  {

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
//#id b88aa065-df8d-4ba1-8a4b-218c37cc90ec
//#Container
{
//#id 9079bbfa-9d19-42b2-9b18-a648c3d635c9
global::Coreflow.ConsoleWriteLineActivity __cc_9079bbfa_9d19_42b2_9b18_a648c3d635c9 = new global::Coreflow.ConsoleWriteLineActivity();

{
//#id 9079bbfa-9d19-42b2-9b18-a648c3d635c9
__cc_9079bbfa_9d19_42b2_9b18_a648c3d635c9.Execute(

//#id c86cb032-876f-482e-9020-77b344724117
"init flow START"
);


//#id 38c46ccf-e18c-42c0-bc23-694e4855f75b
//#Container
{
//#id eb8aed67-519d-4cf7-b16e-62d189c22797
global::Coreflow.SleepActivity __cc_eb8aed67_519d_4cf7_b16e_62d189c22797 = new global::Coreflow.SleepActivity();
while (
//#id 70a24d36-1af1-4368-ae11-cd6988a5dd9b
true
)

{
//#id c0ff7ed2-2672-4d98-8b19-0e742255d2b6
//#id ecb699cd-cf3d-4517-9d7f-b09acd584ef3
Console.WriteLine("" + System.Diagnostics.Debugger.IsAttached);


//#id c4f0ed2a-f04f-4031-b08a-2de462b22f43
__cc_9079bbfa_9d19_42b2_9b18_a648c3d635c9.Execute(

//#id 059b52e9-b547-47b8-a062-a1ca7aef58db
"tick: " + DateTime.Now.Millisecond
);


//#id eb8aed67-519d-4cf7-b16e-62d189c22797
__cc_eb8aed67_519d_4cf7_b16e_62d189c22797.Execute(

//#id afc499c0-01d5-4488-9fa4-efb282eda0da
3000
);


}
}


/* !!! --------------------------------------------------------------- !!!*/
}
}
} /* Run */
} /* Class */
} /* Namespace */
