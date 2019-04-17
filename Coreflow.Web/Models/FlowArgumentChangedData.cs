using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coreflow.Web.Models
{
    public class FlowArgumentChangedData
    {
        public bool AddValue { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }
}
