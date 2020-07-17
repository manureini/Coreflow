using Coreflow.Validation.Messages;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Coreflow.Validation
{
    public abstract class AbstractCorrector : ICorrector
    {
        public FlowDefinition FlowDefinition;

        public List<IFlowValidationMessage> Messages;

        public IFlowValidationMessage Message;

        public abstract string Name { get; }

        protected AbstractCorrector(FlowDefinition pFlowDefinition, List<IFlowValidationMessage> pMessages, IFlowValidationMessage pMessage)
        {
            FlowDefinition = pFlowDefinition;
            Messages = pMessages;
            Message = pMessage;
        }

        public string GetSerializedData()
        {
            return JsonConvert.SerializeObject(new CorrectorData()
            {
                Data = GetData(),
                Type = this.GetType().AssemblyQualifiedName,
                CodeCreators = Message is IFlowValidationCodeCreatorMessage ccmsg ? ccmsg.CodeCreatorIdentifiers : null
            },
            new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }

        public abstract bool CanCorrect();

        public abstract object GetData();
    }
}
