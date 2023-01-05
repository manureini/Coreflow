using System;

namespace Coreflow.Objects
{
    public class DisplayMetaAttribute : Attribute
    {
        public const string DEFAULT_ICON = "fa-gear";

        public string DisplayName { get; }

        public string Icon { get; }

        public string Category { get; }

        public DisplayMetaAttribute(string pDisplayName, string pCategory = null, string pIcon = DEFAULT_ICON)
        {
            Category = pCategory;
            DisplayName = pDisplayName;
            Icon = pIcon;
        }
    }
}
