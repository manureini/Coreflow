namespace Coreflow.Web.Models.Requests
{
    public class ParameterChangedData : FlowEditorRequest
    {
        public string CreatorGuid { get; set; }

        public string ParameterGuid { get; set; }

        public string NewValue { get; set; }
    }
}
