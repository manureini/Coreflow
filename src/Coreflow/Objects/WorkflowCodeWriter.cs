using Coreflow.Helper;
using Coreflow.Interfaces;
using System;
using System.Text;

namespace Coreflow.Objects
{
    public class WorkflowCodeWriter
    {
        private readonly StringBuilder mTopStringBuilder = new StringBuilder();
        private readonly StringBuilder mBottomStringBuilder = new StringBuilder();

        internal WorkflowCodeWriter() { }

        public void AppendTop(string pCode)
        {
            mTopStringBuilder.Append(pCode);
        }

        public void AppendLineTop(string pCode = "")
        {
            AppendTop(Environment.NewLine + pCode);
        }

        public void AppendBottom(string pCode)
        {
            mBottomStringBuilder.Append(pCode);
        }

        public void AppendLineBottom(string pCode = "")
        {
            AppendBottom(Environment.NewLine + pCode);
        }

        public void WriteIdentifierTagTop(IIdentifiable pIdentifiable)
        {
            AppendLineTop(WorkflowCompilerHelper.COMMENT_ID_PREFIX + pIdentifiable.Identifier);
        }

        public void WriteContainerTagTop(IIdentifiable pIdentifiable)
        {
            AppendLineTop(WorkflowCompilerHelper.CONTAINER_ID_PREFIX);
        }

        public void RemoveLastCharTop()
        {
            mTopStringBuilder.Remove(mTopStringBuilder.Length - 1, 1);
        }

        public string ToStringTop()
        {
            return mTopStringBuilder.ToString();
        }

        public string ToStringBottom()
        {
            return mBottomStringBuilder.ToString();
        }

        public override string ToString()
        {
            return mTopStringBuilder.ToString() + mBottomStringBuilder.ToString();
        }
    }
}
