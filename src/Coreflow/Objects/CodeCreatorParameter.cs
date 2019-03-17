using System;

namespace Coreflow.Objects
{
    public class CodeCreatorParameter
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public Type Type { get; set; }

        public string Category { get; set; } = "Default";

        public ParameterDirection Direction { get; set; }

        public CodeCreatorParameter()
        {
        }

        public CodeCreatorParameter(string pName, string pDisplayName, Type pType, string pCategory, ParameterDirection pDirection)
        {
            Name = pName;
            DisplayName = pDisplayName;
            Type = pType;
            Category = pCategory;
            Direction = pDirection;
        }

        public string GetDisplayNameOrName()
        {
            if (!string.IsNullOrWhiteSpace(DisplayName))
                return DisplayName;
            return Name;
        }
    }
}
