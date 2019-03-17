using Coreflow.Helper;
using Coreflow.Interfaces;
using System;
using System.Text;

namespace Coreflow.Objects
{
    public class WorkflowCodeWriter
    {
        public event EventHandler<WorkflowCodeWriterWriteEventArgs> OnCodeWrite = delegate { };

        private StringBuilder mStringBuilder = new StringBuilder();

        internal WorkflowCodeWriter() { }

        public void Append(string pCode)
        {
            WorkflowCodeWriterWriteEventArgs args = new WorkflowCodeWriterWriteEventArgs()
            {
                Code = pCode
            };

            OnCodeWrite.Invoke(this, args);

            mStringBuilder.Append(args.Code);
        }

        public void AppendLine(string pCode = "")
        {
            Append(Environment.NewLine + pCode);
        }

        public void WriteIdentifierTag(IIdentifiable pIdentifiable)
        {
            AppendLine(WorkflowCompilerHelper.COMMENT_ID_PREFIX + pIdentifiable.Identifier);
        }

        public void WriteContainerTag(IIdentifiable pIdentifiable)
        {
            AppendLine(WorkflowCompilerHelper.CONTAINER_ID_PREFIX);
        }

        public void RemoveLastChar()
        {
            mStringBuilder.Remove(mStringBuilder.Length - 1, 1);
        }

        public override string ToString()
        {
            return mStringBuilder.ToString();
        }
    }
}
