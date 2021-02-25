using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;

namespace Coreflow.Blazor
{
    public static class Extensions
    {
        public static string GetDisplayName(this ICodeCreator pCodeCreator, FlowDefinition pFlowDefinition = null)
        {
            if (pCodeCreator == null)
                return null;

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

        public static (ICodeCreatorContainerCreator parent, int indexContext) FindParentOf(this FlowDefinition pFlowDefinition, ICodeCreator pCodeCreator, ICodeCreator pParent = null)
        {
            if (pFlowDefinition == null)
                return (null, -1);

            var parent = pParent;

            if (parent == null)
            {
                parent = pFlowDefinition.CodeCreator;
                if (parent == pCodeCreator)
                    return (null, 0);
            }

            var container = parent as ICodeCreatorContainerCreator;

            if (container == null || container.CodeCreators == null)
                return (null, -1);

            for (int i = 0; i < container.CodeCreators.Count; i++)
            {
                List<ICodeCreator> childContexts = container.CodeCreators[i];

                foreach (var child in childContexts)
                {
                    if (child == pCodeCreator)
                        return (container, i);

                    var result = FindParentOf(pFlowDefinition, pCodeCreator, child);

                    if (result.indexContext != -1)
                        return result;
                }
            }

            return (null, -1);
        }

    }
}
