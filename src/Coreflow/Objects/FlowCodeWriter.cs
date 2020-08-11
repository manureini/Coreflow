using Coreflow.Helper;
using Coreflow.Runtime;
using System;
using System.Linq;
using System.Text;

namespace Coreflow.Objects
{
    public class FlowCodeWriter
    {
        public const string CURSOR_LOC = "/* !!! ------------------------- # CodeWriter Cursor # -------------------------------------- !!!*/";

        private StringBuilder mTopStringBuilder = new StringBuilder();
        private StringBuilder mBottomStringBuilder = new StringBuilder();

        internal FlowCodeWriter() { }

        public void SetTopString(string pString)
        {
            mTopStringBuilder = new StringBuilder(pString);
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
            mBottomStringBuilder.Remove(pIndex, mBottomStringBuilder.Length - pIndex);
        }

        public void RemoveTop(int pIndex)
        {
            mTopStringBuilder.Remove(pIndex, mBottomStringBuilder.Length - pIndex);
        }

        public int GetButtomIndex()
        {
            return mBottomStringBuilder.Length;
        }

        public int GetTopIndex()
        {
            return mTopStringBuilder.Length;
        }

        public string SubstringButtom(int pStartIndex)
        {
            return Reverse(mBottomStringBuilder.ToString().Substring(pStartIndex));
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
            return ToStringTop() + Environment.NewLine + CURSOR_LOC + Environment.NewLine + ToStringBottom();
        }
    }
}
