using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow.Web.Extensions
{
    public static class CodeCreatorExtensions
    {
        public static string GetDisplayName(this ICodeCreator pCodeCreator)
        {
            if (pCodeCreator is IUiDesignable desingable)
                return desingable.Name;

            return pCodeCreator.GetType().Name;
        }

        public static string GetIconClassName(this ICodeCreator pCodeCreator)
        {
            if (pCodeCreator is IUiDesignable desingable)
            {
                return desingable.Icon.GetIconClassName();
            }

            return "fa " + DisplayMetaAttribute.DEFAULT_ICON;
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
