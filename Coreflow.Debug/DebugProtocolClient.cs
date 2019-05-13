using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;
using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Coreflow.Debug
{
    public class DebugProtocolClient : DebugProtocol
    {
        public DebugProtocolClient(Stream readStream, Stream writeStream) : base(readStream, writeStream)
        {
        }

        public DebugProtocolClient(Stream readStream, Stream writeStream, DebugProtocolOptions options) : base(readStream, writeStream, options)
        {

            this.LogMessage += DebugProtocolClient_LogMessage;
        }

        public void SendMessage(ProtocolMessage message)
        {



        }


        private void DebugProtocolClient_LogMessage(object sender, Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.LogEventArgs e)
        {

        }

        protected override void OnEventReceived(string eventType, DebugEvent body)
        {

        }

    }
}
