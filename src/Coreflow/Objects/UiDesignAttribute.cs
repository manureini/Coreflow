using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Objects
{
    public class UiDesignAttribute : Attribute
    {
        public const string DEFAULT_ICON = "fa-cog";

        public string DisplayName { get; }

        public string Icon { get; }

        public UiDesignAttribute(string pDisplayName, string pIcon = DEFAULT_ICON)
        {
            DisplayName = pDisplayName;
            Icon = pIcon;
        }
    }
}
