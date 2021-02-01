using Coreflow.Interfaces;
using System;

namespace Coreflow.Objects.ParameterVariables
{
    public class PlaceholderArgument : IArgument
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public Guid Identifier { get; set; }

        public PlaceholderArgument(string pName, string pCode, Guid pIdentifier)
        {
            Name = pName;
            Code = pCode;
            Identifier = pIdentifier;
        }

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
        }
    }
}
