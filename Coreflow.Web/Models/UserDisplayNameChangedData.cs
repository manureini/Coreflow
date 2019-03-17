using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coreflow.Web.Models
{
    public class UserDisplayNameChangedData
    {
        public string CreatorGuid { get; set; }

        public string NewValue { get; set; }
    }
}
