namespace Coreflow.Web.Models.Requests
{
    public class FlowArgumentChangedData : FlowEditorRequest
    {
        public bool AddValue { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }
}
