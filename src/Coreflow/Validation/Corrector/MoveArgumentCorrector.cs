using Coreflow.Validation.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coreflow.Validation.Corrector
{
    public class MoveArgumentCorrector : AbstractCorrector
    {
        public override string Name => $"Move {((ArgumentButNoParameterMessage)Message).Argument.Name} to {mOtherMsg.Parameter.Name}";

        private ParameterButNoArgumentMessage mOtherMsg;

        public MoveArgumentCorrector(FlowDefinition pFlowDefinition, List<IFlowValidationMessage> pMessages, IFlowValidationMessage pMessage) : base(pFlowDefinition, pMessages, pMessage)
        {
        }

        public override bool CanCorrect()
        {
            if (Message.MessageType != FlowValidationMessageType.ArgumentButNoParameter)
                return false;

            var ccmsg = Message as ArgumentButNoParameterMessage;

            mOtherMsg = (ParameterButNoArgumentMessage)Messages.FirstOrDefault(m => m is ParameterButNoArgumentMessage pm && Enumerable.SequenceEqual(pm.CodeCreatorIdentifiers, ccmsg.CodeCreatorIdentifiers));

            return mOtherMsg != null;
        }

        public override object GetData()
        {
            var ccmsg = Message as ArgumentButNoParameterMessage;
            return (ccmsg.Argument.Name, mOtherMsg.Parameter.Name);
        }

        public static void Correct(FlowDefinition pFlowDefinition, List<Guid> pCodeCreators, object pData)
        {



        }

    }
}
