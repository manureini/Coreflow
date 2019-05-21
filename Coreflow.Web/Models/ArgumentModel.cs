using System;

namespace Coreflow.Web.Models
{
    public class ArgumentModel
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Code { get; set; }

        public ArgumentModel()
        {
        }

        public ArgumentModel(Guid pGuid, string pName, string pType, string pCode)
        {
            Guid = pGuid;
            Name = pName;
            Type = pType;
            Code = pCode;
        }
    }
}
