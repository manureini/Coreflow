using Coreflow.Helper;
using Coreflow.Interfaces;
using System;
using System.Linq;
using System.Text;

namespace Coreflow.Objects
{
    public class WorkflowCodeWriter
    {
        private StringBuilder mTopStringBuilder = new StringBuilder();
        private StringBuilder mBottomStringBuilder = new StringBuilder();

        internal WorkflowCodeWriter() { }

        public void SetStringBuilder(StringBuilder pTmpTopStringBuilder, StringBuilder pTmpBottomStringBuilder)
        {
            mTopStringBuilder = pTmpTopStringBuilder;
            mBottomStringBuilder = pTmpBottomStringBuilder;
        }

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

        public void ReplaceTop(string pOldValue, string pNewValue)
        {
            string top = ToStringTop();
            top = top.Replace(pOldValue, pNewValue);
            mTopStringBuilder = new StringBuilder(top);
        }

        public void ReplaceBottom(string pOldValue, string pNewValue)
        {
            string bottom = ToStringBottom();
            bottom = bottom.Replace(pOldValue, pNewValue);
            mBottomStringBuilder = new StringBuilder(bottom);
        }

        public string ToStringTop()
        {
            return mTopStringBuilder.ToString();
        }

        public string ToStringBottom()
        {
            return string.Join(Environment.NewLine, mBottomStringBuilder.ToString().Split(Environment.NewLine).Reverse());
        }

        public override string ToString()
        {
            return ToStringTop() + ToStringBottom();
        }
    }
}
