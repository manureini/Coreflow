using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Coreflow.Helper
{
    public static class WorkflowInvoker
    {
        public static WorkflowInvokeResult Invoke(WorkflowDefinition pWorkflow, Dictionary<string, object> pArguments = null)
        {
            WorkflowInvokeResult ret = new WorkflowInvokeResult();

            WorkflowCode code = pWorkflow.GenerateWorkflowCode();

            ret.CompileResult = code.Compile();

            if (!ret.CompileResult.Successful)
                throw new Exception("Workflow does not compile: " + ret.CompileResult.ErrorMessage);

            IEnumerable<Type> workflows = ret.CompileResult.ResultAssembly.GetTypes().Where(t => typeof(ICompiledWorkflow).IsAssignableFrom(t));

            foreach (Type wftype in workflows)
            {
                ICompiledWorkflow workflow = Activator.CreateInstance(wftype) as ICompiledWorkflow;

                if (pArguments != null)
                    foreach (var entry in pArguments)
                    {
                        FieldInfo fi = workflow.GetType().GetField(entry.Key);

                        if (fi == null)
                            throw new ArgumentException($"Workflow does not have a parameter with name {entry.Key}");

                        try
                        {
                            fi.SetValue(workflow, entry.Value);
                        }
                        catch (Exception e)
                        {
                            throw new ArgumentException($"Failed to set value for parameter {entry.Key}", e);
                        }
                    }

                workflow.Run();
                ret.ExecutedInstance = workflow;
            }

            return ret;
        }
    }
}
