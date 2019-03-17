using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.Web.Models.Responses
{
    public class GuidListResponse : Response
    {
        public List<GuidEntry> ListValues { get; set; }

        public GuidListResponse() { }

        public GuidListResponse(bool isSuccess, string message, IDictionary<Guid, string> pCompileErrors) : base(isSuccess, message)
        {
            ListValues = pCompileErrors?.Select(e => new GuidEntry(e.Key, e.Value)).ToList();
        }
    }

    public class GuidEntry
    {
        public string Guid { get; }

        public string Value { get; }

        public GuidEntry(Guid pGuid, string pValue)
        {
            Guid = pGuid.ToString();
            Value = pValue;
        }

    }
}
