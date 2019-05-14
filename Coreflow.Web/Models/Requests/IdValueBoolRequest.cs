using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coreflow.Web.Models.Requests
{
    public class IdValueBoolRequest : IdValueRequest
    {
        public bool Bool { get; set; }
    }
}
