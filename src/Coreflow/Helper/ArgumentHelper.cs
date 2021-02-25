using Coreflow.Interfaces;
using Coreflow.Objects;
using System;

namespace Coreflow.Helper
{
    public static class ArgumentHelper
    {
        public static IArgument CreateArgument(CodeCreatorParameter param)
        {
            if (param == null)
                throw new ArgumentNullException();

            if (param.Direction == VariableDirection.In)
                return new InputExpressionCreator(param.Name, param.Type, param.DefaultValueCode);

            if (param.Direction == VariableDirection.Out)
                return new OutputExpressionCreator(param.Name, param.Type);

            //InOut
            return new InputOutputVariableNameCreator(param.Name);
        }        

    }
}
