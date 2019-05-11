using System.Collections.Generic;
using System.Linq;

namespace Coreflow.Web.Models.Responses
{
    public class DictionaryResponse : Response
    {
        public List<ListEntry> ListValues { get; set; }

        public DictionaryResponse() { }

        public DictionaryResponse(bool isSuccess, string message, IDictionary<string, string> pDictionary) : base(isSuccess, message)
        {
            ListValues = pDictionary?.Select(e => new ListEntry(e.Key, e.Value)).ToList();
        }
    }

    public class ListEntry
    {
        public string Guid { get; }

        public string Value { get; }

        public ListEntry(string pKey, string pValue)
        {
            Guid = pKey;
            Value = pValue;
        }

    }
}
