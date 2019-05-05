namespace Coreflow.Web.Models.Requests
{
    public class FlowReferencedNamespaceChangedData : FlowEditorRequest
    {
        public bool AddValue { get; set; }

        public string Value { get; set; }
    }
}
