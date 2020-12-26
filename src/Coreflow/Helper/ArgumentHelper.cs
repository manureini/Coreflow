using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Objects.ParameterVariables;
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
                return new InputExpressionCreator(param.Name, param.Type.AssemblyQualifiedName, param.DefaultValueCode);

            if (param.Direction == VariableDirection.Out)
                return new OutputExpressionCreator(param.Name, param.Type.AssemblyQualifiedName);

            //InOut
            return new InputOutputVariableNameCreator(param.Name);

        }

        [Obsolete]
        public static IArgument CreateArgument(CodeCreatorParameter param, string pName, string pType, string pCode, Guid pIdentifier)
        {
            if (param == null)
                return new PlaceholderArgument(pName, pCode, pIdentifier);

            if (param.Direction == VariableDirection.In)
                return new InputExpressionCreator(pName, pCode, pIdentifier, pType, param.DefaultValueCode);
            else if (param.Direction == VariableDirection.Out)
            {
                /*
                bool isSimpleVariableName = !pCode.Trim().Contains(" ") && !pCode.Contains("\"");

                if (isSimpleVariableName)
                {
                    //   pName = pName.Trim();

                    if (param.Type == typeof(LeftSideCSharpCode))
                        return new LeftSideVariableNameCreator(pName, pCode, pIdentifier);
                    else
                        return new OutputVariableNameCreator(pName, pCode, pIdentifier);
                }
                else
                    return new OutputVariableCodeInlineCreator(pName, pCode, pIdentifier); */

                return new OutputExpressionCreator(param.Name, param.Type.AssemblyQualifiedName)
                {
                    Code = pCode
                };
            }
            else //InOut
            {
                return new InputOutputVariableNameCreator(pName, pCode, pIdentifier);
            }
        }

    }
}
