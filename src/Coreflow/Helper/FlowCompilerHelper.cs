using Coreflow.Objects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Coreflow.Helper
{
    public static class FlowCompilerHelper
    {
        public const string COMMENT_ID_PREFIX = "//#id ";
        public const string CONTAINER_ID_PREFIX = "//#Container";

        public const string FLOW_ASSEMBLY_PREFIX = "Generated_Flows_";

        private static Regex mIdRegex = new Regex(@"\/\/#id *([a-zA-Z0-9-]*)");
        private static Regex mContainerRegex = new Regex(@"\/\/#Container");

        private static volatile int mCounter = 0;

        public static ReferenceHelper ReferenceHelper = new ReferenceHelper(a => a.FullName.StartsWith(FlowCompilerHelper.FLOW_ASSEMBLY_PREFIX));

        public static FlowCompileResult CompileFlowCode(string pCode, bool pDebug, string pTmpDir)
        {
            FlowCompileResult ret = new FlowCompileResult();

            var assemblyName = FLOW_ASSEMBLY_PREFIX + Guid.NewGuid().ToString().Replace("-", "_");

            string dllPath = null;
            string pdbPath = null;
            string sourcePath = null;
            byte[] assembly = null;

            EmitResult emitResult = Emit(pCode, assemblyName, pDebug, pTmpDir, ref dllPath, ref pdbPath, ref sourcePath, ref assembly);

            ret.DllFilePath = dllPath;
            ret.PdbFilePath = pdbPath;
            ret.SourcePath = sourcePath;
            ret.AssemblyBinary = assembly;

            if (!emitResult.Success)
            {
                string[] codeLines = pCode.Split(Environment.NewLine);

                IEnumerable<Diagnostic> failures = emitResult.Diagnostics.Where(
                    diagnostic => diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                string errors = "";

                ret.ErrorCodeCreators = new Dictionary<Guid, string>();

                foreach (Diagnostic diagnostic in failures)
                {
                    int line = diagnostic.Location.GetLineSpan().StartLinePosition.Line;

                    Guid ccGuid = GetIdentifier(codeLines, line);

                    if (!ret.ErrorCodeCreators.ContainsKey(ccGuid))
                    {
                        ret.ErrorCodeCreators.Add(ccGuid, diagnostic.GetMessage());
                    }
                    else
                    {
                        ret.ErrorCodeCreators[ccGuid] += Environment.NewLine + diagnostic.GetMessage();
                    }

                    errors += diagnostic.GetMessage();
                    Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                }

                ret.ErrorMessage = errors;

                return ret;
            }

            ret.Successful = true;
            return ret;
        }

        private static EmitResult Emit(string pCode, string pAssemblyName, bool pDebug, string pTmpDir,
            ref string assemblyPath, ref string pdbPath, ref string sourcePath, ref byte[] pAssembly)
        {
            SourceText sourceText = null;
            SyntaxTree syntaxTree = GetSyntaxTree(pCode, pAssemblyName, pTmpDir, ref assemblyPath, ref pdbPath, ref sourcePath, ref sourceText);

            var references = ReferenceHelper.GetMetadataReferences();

            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithPlatform(Platform.AnyCpu);

            if (pDebug)
            {
                compilationOptions = compilationOptions.WithOptimizationLevel(OptimizationLevel.Debug);
            }

            var compilation = CSharpCompilation.Create(pAssemblyName)
              .WithOptions(compilationOptions)
              .AddReferences(references)
              .AddSyntaxTrees(syntaxTree);

            EmitOptions emitOptions = null;
            List<EmbeddedText> embeddedTexts = null;

            if (pDebug)
            {
                emitOptions = new EmitOptions(
                 debugInformationFormat: DebugInformationFormat.PortablePdb,
                 pdbFilePath: pdbPath //was: pdbFileName
                );

                embeddedTexts = new List<EmbeddedText>();
                embeddedTexts.Add(EmbeddedText.FromSource(sourcePath, sourceText));
            }

            EmitResult emitResult = null;

            using MemoryStream msDll = new();
            using MemoryStream msPdb = new();

            emitResult = compilation.Emit(
               options: emitOptions,
               peStream: msDll,
               pdbStream: msPdb,
               embeddedTexts: embeddedTexts
               );

            msDll.Seek(0, SeekOrigin.Begin);
            msPdb.Seek(0, SeekOrigin.Begin);

            if (pTmpDir != null)
            {
                using Stream fsDll = File.OpenWrite(assemblyPath);
                msDll.CopyTo(fsDll);
                msDll.Flush();
                fsDll.Flush();

                using Stream fsPdb = File.OpenWrite(pdbPath);
                msPdb.CopyTo(fsPdb);
                msPdb.Flush();
                fsPdb.Flush();
            }
            else
            {
                pAssembly = msDll.ToArray();
            }

            return emitResult;
        }

        private static SyntaxTree GetSyntaxTree(string pCode, string pAssemblyName, string pTmpDir, ref string assemblyPath, ref string pdbPath, ref string sourcePath, ref SourceText pSourceText)
        {
            Encoding encoding = Encoding.UTF8;

            var parseOptions = new CSharpParseOptions(kind: SourceCodeKind.Regular, languageVersion: LanguageVersion.Latest);
            SyntaxTree syntaxTree;

            if (pTmpDir != null)
            {
                int compilationNumber = Interlocked.Increment(ref mCounter);

                string pdbFileName = Path.GetFileNameWithoutExtension(pAssemblyName) + "_" + compilationNumber + ".pdb";
                string dllFileName = Path.GetFileNameWithoutExtension(pAssemblyName) + "_" + compilationNumber + ".dll";

                sourcePath = Path.Combine(pTmpDir, "Flows_" + compilationNumber + ".cs");
                assemblyPath = Path.Combine(pTmpDir, dllFileName);
                pdbPath = Path.Combine(pTmpDir, pdbFileName);

                File.WriteAllText(sourcePath, FlowBuilderHelper.FormatCode(pCode));

                var buffer = encoding.GetBytes(File.ReadAllText(sourcePath));
                pSourceText = SourceText.From(buffer, buffer.Length, encoding, canBeEmbedded: true);
                var tree = CSharpSyntaxTree.ParseText(pSourceText, parseOptions, path: sourcePath);

                var syntaxRootNode = tree.GetRoot() as CSharpSyntaxNode;
                syntaxTree = CSharpSyntaxTree.Create(syntaxRootNode, null, sourcePath, encoding);
            }
            else
            {
                syntaxTree = CSharpSyntaxTree.ParseText(pCode, parseOptions);
            }

            return syntaxTree;
        }

        public static Compilation CreateCompilation(string pAssemblyName, SyntaxTree syntaxTree)
        {
            return CreateLibraryCompilation(pAssemblyName, false)
               .AddReferences(ReferenceHelper.GetMetadataReferences())
               .AddSyntaxTrees(syntaxTree)
               ;
        }

        public static SyntaxTree ParseCode(string pSourceCode)
        {
            var options = new CSharpParseOptions(kind: SourceCodeKind.Regular, languageVersion: LanguageVersion.Latest);
            return CSharpSyntaxTree.ParseText(pSourceCode, options);
        }

        internal static Compilation CreateLibraryCompilation(string assemblyName, bool enableOptimisations)
        {
            var options = new CSharpCompilationOptions(
                    OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: enableOptimisations ? OptimizationLevel.Release : OptimizationLevel.Debug,
                    allowUnsafe: true).WithPlatform(Platform.AnyCpu);

            return CSharpCompilation.Create(assemblyName, options: options);
        }

        private static Guid GetIdentifier(string[] pCode, int pLineOfCode)
        {
            if (pLineOfCode >= pCode.Length)
                return Guid.Empty;

            for (pLineOfCode--; pLineOfCode > 0; pLineOfCode--)
            {
                var regexResult = mIdRegex.Match(pCode[pLineOfCode]);
                if (regexResult.Success)
                {
                    return Guid.Parse(regexResult.Groups[1].Value);
                }
            }

            return Guid.Empty;

            //throw new Exception("GetIdentifier: Identifier not found!");
        }

        public static int GetLineOfIdentifier(string pCode, Guid pIdentifier)
        {
            string[] code = pCode.Split(Environment.NewLine);

            string guid = pIdentifier.ToString();

            for (int i = 0; i < code.Length; i++)
            {
                var regexResult = mIdRegex.Match(code[i]);
                if (regexResult.Success && regexResult.Groups[1].Value == guid)
                {
                    return i + 1;
                }
            }

            throw new Exception("GetLineOfCode: Identifier not found!");
        }
    }
}
