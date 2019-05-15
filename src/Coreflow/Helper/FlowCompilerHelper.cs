using Coreflow.Objects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Coreflow.Helper
{
    public static class FlowCompilerHelper
    {
        public const string COMMENT_ID_PREFIX = "//#id ";
        public const string CONTAINER_ID_PREFIX = "//#Container";

        private static Regex mIdRegex = new Regex(@"\/\/#id *([a-zA-Z0-9-]*)");
        private static Regex mContainerRegex = new Regex(@"\/\/#Container");


        private static volatile int mCounter = 0;



        public static FlowCompileResult CompileFlowCode(string pCode, bool pDebug, string pAssemblyName = null)
        {
            FlowCompileResult ret = new FlowCompileResult();


            pAssemblyName ??= Guid.NewGuid().ToString() + ".dll";

            EmitResult emitResult;

            if (pDebug)
            {
                emitResult = EmitWithDebugSymbols(pCode, pAssemblyName, out string dllPath, out string pdbPath, out string sourcePath);

                ret.DllFilePath = dllPath;
                ret.PdbFilePath = pdbPath;
                ret.SourcePath = sourcePath;
            }
            else
            {
                emitResult = EmitNoDebug(pAssemblyName, pCode);
            }

            if (!emitResult.Success)
            {
                string[] codeLines = pCode.Split("\n");

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


        private static EmitResult EmitWithDebugSymbols(string pCode, string pAssemblyName, out string assemblyPath, out string pdbPath, out string sourcePath)
        {
            int compilationNumber = Interlocked.Increment(ref mCounter);

            string tmpDir = Path.GetFullPath("tmp");

            string pdbFileName = Path.GetFileNameWithoutExtension(pAssemblyName) + "_" + compilationNumber + ".pdb";
            string dllFileName = Path.GetFileNameWithoutExtension(pAssemblyName) + "_" + compilationNumber + ".dll";

            sourcePath = Path.Combine(tmpDir, "Flows_" + compilationNumber + ".cs");
            assemblyPath = Path.Combine(tmpDir, dllFileName);
            pdbPath = Path.Combine(tmpDir, pdbFileName);


            File.WriteAllText(sourcePath, FlowBuilderHelper.FormatCode(pCode));

            Encoding encoding = Encoding.UTF8;

            var buffer = encoding.GetBytes(File.ReadAllText(sourcePath));

            SourceText sourceText = SourceText.From(buffer, buffer.Length, encoding, canBeEmbedded: true);

            var tree = CSharpSyntaxTree.ParseText(sourceText, new CSharpParseOptions(), path: sourcePath);

            var syntaxRootNode = tree.GetRoot() as CSharpSyntaxNode;
            var encoded = CSharpSyntaxTree.Create(syntaxRootNode, null, sourcePath, encoding);

            //TODO !!!!!!!!!!!!!

            var references = ReferenceHelper.GetMetadataReferences();

            var compilation = CSharpCompilation.Create(pAssemblyName)
              .WithOptions(
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                  .WithOptimizationLevel(OptimizationLevel.Debug)
                  .WithPlatform(Platform.AnyCpu))
              .AddReferences(references)
              .AddSyntaxTrees(tree);

            var emitOptions = new EmitOptions(
               debugInformationFormat: DebugInformationFormat.PortablePdb,
               pdbFilePath: pdbFileName
              // runtimeMetadataVersion: "1.0"
              );

            var embeddedTexts = new List<EmbeddedText>
            {
                EmbeddedText.FromSource(sourcePath, sourceText),
            };


            EmitResult emitResult = null;

            using (Stream fsDll = File.OpenWrite(assemblyPath))
            using (Stream fsPdb = File.OpenWrite(pdbPath))
            {
                emitResult = compilation.Emit(
                   options: emitOptions,
                   peStream: fsDll,
                   pdbStream: fsPdb,
                   embeddedTexts: embeddedTexts
                   );

                fsDll.Close();
                fsPdb.Close();
            }

            return emitResult;
        }

        private static EmitResult EmitNoDebug(string pAssemblyName, string pCode)
        {
            var tree = ParseTextNotDebuggable(pCode);

            var references = ReferenceHelper.GetMetadataReferences();

            var compilation = CSharpCompilation.Create(pAssemblyName)
              .AddReferences(references)
              .AddSyntaxTrees(tree)
              .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using MemoryStream ms = new MemoryStream();
            EmitResult emitResult = compilation.Emit(ms);
            return emitResult;
        }

        public static Compilation CreateCompilation(string pAssemblyName, SyntaxTree syntaxTree)
        {
            return CreateLibraryCompilation(pAssemblyName, false)
               .AddReferences(ReferenceHelper.GetMetadataReferences())
               .AddSyntaxTrees(syntaxTree)
               ;
        }

        public static SyntaxTree ParseTextNotDebuggable(string pSourceCode)
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

        private static int GetContainerLineOfCode(string[] pCode, int pLineOfCode)
        {
            for (pLineOfCode--; pLineOfCode > 0; pLineOfCode--)
            {
                var regexResult = mContainerRegex.Match(pCode[pLineOfCode]);
                if (regexResult.Success)
                {
                    return pLineOfCode;
                }
            }

            throw new Exception("GetContainer: Container not found!");
        }

    }
}
