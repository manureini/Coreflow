namespace Coreflow.Web.Models.Requests
{
    public class UserDisplayNameChangedData : FlowEditorRequest
    {
        public string CreatorGuid { get; set; }

        public string NewValue { get; set; }
    }
}
