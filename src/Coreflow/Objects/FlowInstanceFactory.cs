using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Coreflow.Objects
{
    public class FlowInstanceFactory
    {
        public delegate ICompiledFlow FlowCreator();


        public Coreflow Coreflow { get; protected set; }

        public Guid DefinitionGuid { get; protected set; }

        protected FlowCreator mCreator;

        internal FlowInstanceFactory(Coreflow pCoreFlow, Guid pDefinitionGuid, Type pType)
        {
            Coreflow = pCoreFlow;
            DefinitionGuid = pDefinitionGuid;

            ConstructorInfo emptyConstructor = pType.GetConstructor(Type.EmptyTypes);
            var dynamicMethod = new DynamicMethod("CreateInstance", pType, Type.EmptyTypes, true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            //   ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);

            mCreator = (FlowCreator)dynamicMethod.CreateDelegate(typeof(FlowCreator));
        }

        public ICompiledFlow NewInstance()
        {
            return mCreator();
        }

        public IDictionary<string, object> RunInstance(IDictionary<string, object> pArguments = null)
        {
            ICompiledFlow flow = NewInstance();

            FlowInstance flowInstance = new FlowInstance()
            {
                DefinitionIdentifier = DefinitionGuid,
                StartTime = DateTime.UtcNow,
                Identifier = flow.InstanceId
            };

            Coreflow.FlowInstanceStorage.Add(flowInstance);

            if (pArguments != null)
                flow.SetArguments(pArguments);

            flow.Run();

            flowInstance.EndTime = DateTime.UtcNow;

            Coreflow.FlowInstanceStorage.Update(flowInstance);
            
            return flow.GetArguments();
        }
    }
}
