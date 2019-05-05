namespace Coreflow.Web.Models.Requests
{
    public class MoveAfterData : FlowEditorRequest
    {
        public string SourceId { get; set; }

        public string DestinationAfterId { get; set; }

        public string DestinationContainerId { get; set; }

        public string SequenceIndex { get; set; }
    }
}
