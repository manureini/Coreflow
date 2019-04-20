using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Coreflow.Helper
{
    public static class FlowInvoker
    {
        public static FlowInvokeResult Invoke(FlowDefinition pFlow, Dictionary<string, object> pArguments = null)
        {
            FlowInvokeResult ret = new FlowInvokeResult();

            /*
            FlowCode code = pFlow.GenerateFlowCode();

            ret.CompileResult = code.Compile();

            if (!ret.CompileResult.Successful)
                throw new Exception("Flow does not compile: " + ret.CompileResult.ErrorMessage);

            IEnumerable<Type> Flows = ret.CompileResult.ResultAssembly.GetTypes().Where(t => typeof(ICompiledFlow).IsAssignableFrom(t));

            foreach (Type wftype in Flows)
            {
                ICompiledFlow Flow = Activator.CreateInstance(wftype) as ICompiledFlow;

                if (pArguments != null)
                    foreach (var entry in pArguments)
                    {
                        FieldInfo fi = Flow.GetType().GetField(entry.Key);

                        if (fi == null)
                            throw new ArgumentException($"Flow does not have a parameter with name {entry.Key}");

                        try
                        {
                            fi.SetValue(Flow, entry.Value);
                        }
                        catch (Exception e)
                        {
                            throw new ArgumentException($"Failed to set value for parameter {entry.Key}", e);
                        }
                    }

                ret.Instance = new FlowInstance()
                {
                    DefinitionIdentifier = pFlow.Identifier,
                    StartTime = DateTime.UtcNow,
                    Identifier = Guid.NewGuid()
                };

                pFlow.Coreflow.FlowInstanceStorage.Add(ret.Instance);

                Flow.Run();

                ret.Instance.EndTime = DateTime.UtcNow;
                pFlow.Coreflow.FlowInstanceStorage.Update(ret.Instance);

                ret.ExecutedInstance = Flow;
            }*/

            return ret;
        }
    }
}
