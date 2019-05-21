using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Objects.ParameterVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Helper
{
    public static class ArgumentHelper
    {


        public static IArgument CreateArgument(CodeCreatorParameter param, string pName, string pCode, Guid pIdentifier)
        {
            if (param == null)
                return new PlaceholderArgument(pName, pCode, pIdentifier);

            if (param.Direction == VariableDirection.In)
                return new InputExpressionCreator(pName, pCode, pIdentifier, param.Type);
            else if (param.Direction == VariableDirection.Out)
            {
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
                    return new OutputVariableCodeInlineCreator(pName, pCode, pIdentifier);
            }
            else //InOut
            {
                return new InputOutputVariableNameCreator(pName, pCode, pIdentifier);
            }

        }

    }
}
