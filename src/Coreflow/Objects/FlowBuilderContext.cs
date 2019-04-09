using Coreflow.Helper;
using Coreflow.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.Objects
{
    public class FlowBuilderContext
    {
        private Dictionary<Guid, string> mLocalObjectNames = new Dictionary<Guid, string>();

        public Dictionary<string, object> BuildingContext = new Dictionary<string, object>();

        private FlowCodeWriter mCodeWriter;
        private List<MetadataReference> mReferencedAssemblies;

        public IEnumerable<ISymbol> CurrentSymbols { get; protected set; }

        public FlowBuilderContext(FlowCodeWriter pCodeWriter, List<MetadataReference> pReferencedAssemblies)
        {
            mCodeWriter = pCodeWriter;
            mReferencedAssemblies = pReferencedAssemblies;
        }


        public string GetOrCreateLocalVariableName(IVariableCreator pVariableCreator)
        {
            if (mLocalObjectNames.ContainsKey(pVariableCreator.Identifier))
                return mLocalObjectNames[pVariableCreator.Identifier];

            string variableName = Guid.NewGuid().ToString().ToVariableName();

            mLocalObjectNames.Add(pVariableCreator.Identifier, variableName);
            return variableName;
        }

        public string CreateLocalVariableName(IVariableCreator pVariableCreator)
        {
            string variableName = Guid.NewGuid().ToString().ToVariableName();
            mLocalObjectNames.Add(pVariableCreator.Identifier, variableName);
            return variableName;
        }

        public void SetLocalVariableName(IVariableCreator pVariableCreator, string pLocalVariableName)
        {
            mLocalObjectNames.Add(pVariableCreator.Identifier, pLocalVariableName);
        }

        public string GetLocalVariableName(IVariableCreator pVariableCreator)
        {
            return mLocalObjectNames[pVariableCreator.Identifier];
        }

        public bool HasLocalVariableName(IVariableCreator pVariableCreator)
        {
            return mLocalObjectNames.ContainsKey(pVariableCreator.Identifier);
        }

        public void UpdateCurrentSymbols()
        {
            string code = mCodeWriter.ToString();
            string[] codeLines = code.Split("\n");

            // int loc = FlowCompilerHelper.GetLineOfCode(codeLines, pIdentifiable.Identifier); 

            string topCode = mCodeWriter.ToStringTop();
            string[] topCodeLines = topCode.Split("\n");

            int loc = topCodeLines.Length;

            SyntaxTree tree = FlowCompilerHelper.ParseText(code);

            SyntaxNode sn = GetNode(tree, loc);

            CurrentSymbols = CompilationLookUpSymbols(tree, sn as CSharpSyntaxNode);
        }

        static SyntaxNode GetNode(SyntaxTree tree, int lineNumber)
        {
            while (lineNumber > 0)
            {
                var lineSpan = tree.GetText().Lines[lineNumber].Span;
                SyntaxNode ret = tree.GetRoot().DescendantNodes(lineSpan).FirstOrDefault(n => lineSpan.Contains(n.Span));

                if (ret != null)
                    return ret;

                lineNumber--;
            }

            return null;
        }


        static IEnumerable<ISymbol> CompilationLookUpSymbols(SyntaxTree tree, CSharpSyntaxNode currentNode)
        {
            var compilation = CSharpCompilation.Create("dummy", new[] { tree });
            var model = compilation.GetSemanticModel(tree);
            return model.LookupSymbols(currentNode.SpanStart);
        }



    }
}
