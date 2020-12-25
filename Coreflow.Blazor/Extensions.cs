using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreflow.Blazor
{
    public static class Extensions
    {
        public static string GetDisplayName(this ICodeCreator pCodeCreator, FlowDefinition pFlowDefinition)
        {
            if (pFlowDefinition != null)
            {
                if (pFlowDefinition.GetMetadata(pCodeCreator.Identifier, Constants.USER_DISPLAY_NAME) is string userDisplayName && !string.IsNullOrWhiteSpace(userDisplayName))
                    return userDisplayName;
            }

            if (pCodeCreator is IUiDesignable designable && !string.IsNullOrWhiteSpace(designable.Name))
                return designable.Name;

            return pCodeCreator.GetType().Name;
        }

        public static string GetIconClassName(this ICodeCreator pCodeCreator)
        {
            if (pCodeCreator is IUiDesignable desingable)
            {
                return GetIconClassName(desingable.Icon);
            }

            return "fa " + DisplayMetaAttribute.DEFAULT_ICON;
        }

        public static string GetIconClassName(string pIconString)
        {
            if (pIconString == null)
                return "fa " + DisplayMetaAttribute.DEFAULT_ICON;

            if (pIconString.Contains("fa-"))
                return "fa " + pIconString;

            throw new NotSupportedException($"Icon '{pIconString}' is not supported");
        }

        public static string GetCategory(this ICodeCreator pCodeCreator)
        {
            if (pCodeCreator is IUiDesignable desingable)
            {
                return desingable.Category;
            }

            return null;
        }
    }
}
