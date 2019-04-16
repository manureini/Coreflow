using Coreflow.Helper;
using Coreflow.Interfaces;
using System;
using System.Linq;
using System.Text;

namespace Coreflow.Objects
{
    public class FlowCodeWriter
    {
        private StringBuilder mTopStringBuilder = new StringBuilder();
        private StringBuilder mBottomStringBuilder = new StringBuilder();

        internal FlowCodeWriter() { }

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
            AppendLineTop(FlowCompilerHelper.COMMENT_ID_PREFIX + pIdentifiable.Identifier);
        }

        public void WriteContainerTagTop(IIdentifiable pIdentifiable)
        {
            AppendLineTop(FlowCompilerHelper.CONTAINER_ID_PREFIX);
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
            string bottom = mBottomStringBuilder.ToString();
            bottom = bottom.Replace(pOldValue, pNewValue);
            mBottomStringBuilder = new StringBuilder(bottom);
        }

        public void RemoveBottom(int pIndex)
        {
            /*     string bottom = ToStringBottom();
                 bottom = bottom.Remove(pIndex, mBottomStringBuilder.Length - pIndex);
                 mBottomStringBuilder = new StringBuilder(bottom);*/

            mBottomStringBuilder.Remove(pIndex, mBottomStringBuilder.Length - pIndex);
        }

        public int GetButtomIndex()
        {
            return mBottomStringBuilder.Length;
        }

        public string SubstringButtom(int pIndex)
        {
            return Reverse(mBottomStringBuilder.ToString().Substring(pIndex));
        }

        public string ToStringTop()
        {
            return mTopStringBuilder.ToString();
        }

        public string ToStringBottom()
        {
            return Reverse(mBottomStringBuilder.ToString());
        }

        private string Reverse(string pStr)
        {
            return string.Join(Environment.NewLine, pStr.Split(Environment.NewLine).Reverse());
        }

        public override string ToString()
        {
            return ToStringTop() + Environment.NewLine + Environment.NewLine + ToStringBottom();
        }
    }
}
