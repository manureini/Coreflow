using Coreflow.Interfaces;
using Coreflow.Validation.Messages;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.Validation.Corrector
{
    public class MoveArgumentCorrector : AbstractCorrector
    {
        public override string Name => $"Rename {((ArgumentButNoParameterMessage)Message).Argument.Name} to {mOtherMsg.Parameter.Name}";

        private ParameterButNoArgumentMessage mOtherMsg;

        public MoveArgumentCorrector(FlowDefinition pFlowDefinition, List<IFlowValidationMessage> pMessages, IFlowValidationMessage pMessage) : base(pFlowDefinition, pMessages, pMessage)
        {
        }

        public override bool CanCorrect()
        {
            if (!(Message is ArgumentButNoParameterMessage))
                return false;

            var ccmsg = Message as ArgumentButNoParameterMessage;

            mOtherMsg = (ParameterButNoArgumentMessage)Messages.FirstOrDefault(m => m is ParameterButNoArgumentMessage pm && Enumerable.SequenceEqual(pm.CodeCreatorIdentifiers, ccmsg.CodeCreatorIdentifiers));

            return mOtherMsg != null;
        }

        public override object GetData()
        {
            var ccmsg = Message as ArgumentButNoParameterMessage;

            return new MoveDataContainer()
            {
                ArgumentName = ccmsg.Argument.Name,
                ParameterName = mOtherMsg.Parameter.Name
            };
        }

        public static void Correct(FlowDefinition pFlowDefinition, List<Guid> pCodeCreators, object pData)
        {
            var data = ((JObject)pData).ToObject<MoveDataContainer>();

            foreach (var ccid in pCodeCreators)
            {
                var cc = pFlowDefinition.FindCodeCreator(ccid) as IParametrized;
                var argument = cc.Arguments.First(a => a.Name == data.ArgumentName);
                argument.Name = data.ParameterName;
            }
        }
    }


    public class MoveDataContainer
    {
        public string ArgumentName { get; set; }

        public string ParameterName { get; set; }
    }

}
