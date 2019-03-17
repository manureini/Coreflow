using Coreflow.Interfaces;
using System;
using System.Collections.Generic;

namespace Coreflow.Objects
{
    public class WorkflowBuilderContext
    {
        private Dictionary<Guid, string> mLocalObjectNames = new Dictionary<Guid, string>();

        public Dictionary<string, object> BuildingContext = new Dictionary<string, object>();

        public string GetOrCreateLocalVariableName(IVariableCreator pVariableCreator)
        {
            if (mLocalObjectNames.ContainsKey(pVariableCreator.Identifier))
                return mLocalObjectNames[pVariableCreator.Identifier];

            string variableName = Guid.NewGuid().ToString().ToVariableName();

            mLocalObjectNames.Add(pVariableCreator.Identifier, variableName);
            return variableName;
        }

        public string CreateLocalVariableName(IVariableCreator pVariableCreator)
        {
            string variableName = Guid.NewGuid().ToString().ToVariableName();
            mLocalObjectNames.Add(pVariableCreator.Identifier, variableName);
            return variableName;
        }

        public void SetLocalVariableName(IVariableCreator pVariableCreator, string pLocalVariableName)
        {
            mLocalObjectNames.Add(pVariableCreator.Identifier, pLocalVariableName);
        }

        public string GetLocalVariableName(IVariableCreator pVariableCreator)
        {
            return mLocalObjectNames[pVariableCreator.Identifier];
        }

        public bool HasLocalVariableName(IVariableCreator pVariableCreator)
        {
            return mLocalObjectNames.ContainsKey(pVariableCreator.Identifier);
        }
    }
}
