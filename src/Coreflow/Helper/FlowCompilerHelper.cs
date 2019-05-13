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

namespace Coreflow.Helper
{
    public static class FlowCompilerHelper
    {
        public const string COMMENT_ID_PREFIX = "//#id ";
        public const string CONTAINER_ID_PREFIX = "//#Container";
        public const string LAST_COMPILED_CODE_FILE = "LastCompiledCode.cs";
        public const string LAST_COMPILED_CODE_FILE_FORMATTED = "LastCompiledCodeFormatted.cs";

        private static Regex mIdRegex = new Regex(@"\/\/#id *([a-zA-Z0-9-]*)");
        private static Regex mContainerRegex = new Regex(@"\/\/#Container");


        public static FlowCompileResult CompileFlowCode(string pCode, bool pDebug, string pAssemblyName = null)
        {
            FlowCompileResult ret = new FlowCompileResult();

            File.Delete(LAST_COMPILED_CODE_FILE);
            File.Delete(LAST_COMPILED_CODE_FILE_FORMATTED);

            SyntaxTree syntaxTree = ParseText(pCode, pDebug);

            //  string formattedCode = FormatCode(syntaxTree);        

            pAssemblyName ??= Guid.NewGuid().ToString();

            Compilation compilation = CreateCompilation(pAssemblyName, syntaxTree);

            var emitOptions = new EmitOptions(
                 debugInformationFormat: DebugInformationFormat.PortablePdb,
                // pdbFilePath: pAssemblyName + ".pdb"
                runtimeMetadataVersion: "1.0"
                );


          
            var assemblyStream = new MemoryStream();
            var symbolsStream = new MemoryStream();

            var emitResult = compilation.Emit(
                peStream: assemblyStream,
                pdbStream: symbolsStream,
                options: emitOptions);

            ret.ResultAssembly = assemblyStream;
            ret.ResultSymbols = symbolsStream;

            string[] codeLines = pCode.Split("\n");

            if (!emitResult.Success)
            {
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


            /*
            Dictionary<Guid, List<string>> containerVariables = new Dictionary<Guid, List<string>>();

            void addToContainerVariables(Guid pContainerGuid, string pVariableIdentifier)
            {
                if (pVariableIdentifier.IsContainerCreatorVariableName())
                    return;

                if (containerVariables.ContainsKey(pContainerGuid))
                    containerVariables[pContainerGuid].Add(pVariableIdentifier);
                else
                {
                    containerVariables.Add(pContainerGuid, new List<string>());
                    containerVariables[pContainerGuid].Add(pVariableIdentifier);
                }
            };

            var root = (CompilationUnitSyntax)syntaxTree.GetRoot();

            var variableDeclarations = root.DescendantNodes().OfType<VariableDeclarationSyntax>();

            foreach (var variableDeclaration in variableDeclarations)
            {
                string identifier = variableDeclaration.Variables.First().Identifier.Value.ToString();

                int line = variableDeclaration.GetLocation().GetLineSpan().StartLinePosition.Line;
                int containerLine = GetContainerLineOfCode(codeLines, line);
                Guid containerGuid = GetIdentifier(codeLines, containerLine);

                addToContainerVariables(containerGuid, identifier);
            }

            var variableDeclarationsParams = root.DescendantNodes().OfType<SingleVariableDesignationSyntax>();

            foreach (var variableDeclaration in variableDeclarationsParams)
            {
                string identifier = variableDeclaration.Identifier.Value.ToString();

                int line = variableDeclaration.GetLocation().GetLineSpan().StartLinePosition.Line;
                int containerLine = GetContainerLineOfCode(codeLines, line);
                Guid containerGuid = GetIdentifier(codeLines, containerLine);

                addToContainerVariables(containerGuid, identifier);
            }

    */



            ret.Successful = true;

            /*
            Directory.CreateDirectory("tmp");
            string asmFilename = Path.Combine("tmp", assemblyName + ".dll");

            File.WriteAllBytes(asmFilename, stream.ToArray());

            ret.ResultAssembly = Assembly.LoadFile(Path.GetFullPath(asmFilename));
            */


            /*
            Guid flowIdentifier = pFlowCode.Definition.Identifier;

            if (mGeneratedAssemblies.ContainsKey(flowIdentifier))
            {
                mGeneratedAssemblies.Remove(flowIdentifier);
                //try to unload old assembly
            }

            mGeneratedAssemblies.Add(flowIdentifier, MetadataReference.CreateFromFile(asmFilename));


            IEnumerable<Type> flows = ret.ResultAssembly.GetTypes().Where(t => typeof(ICompiledFlow).IsAssignableFrom(t));

            Type flowType = flows.First();

            ret.InstanceFactory = new FlowInstanceFactory(pFlowCode.Definition.Coreflow, pFlowCode.Definition.Identifier, flowType);
            */


            return ret;
        }

        public static Compilation CreateCompilation(string pAssemblyName, SyntaxTree syntaxTree)
        {           
            return CreateLibraryCompilation(pAssemblyName, false)
               .AddReferences(ReferenceHelper.GetMetadataReferences())
               .AddSyntaxTrees(syntaxTree)
               ;
        }

        public static SyntaxTree ParseText(string pSourceCode, bool pDebug)
        {
            var options = new CSharpParseOptions(kind: SourceCodeKind.Regular, languageVersion: LanguageVersion.Latest);

            if (pDebug)
            {
                //  string file = Path.GetFileNameWithoutExtension(LAST_COMPILED_CODE_FILE) + DateTime.Now.Millisecond + ".cs";

                string file = LAST_COMPILED_CODE_FILE;
                File.WriteAllText(file, pSourceCode);

                using var stream = File.OpenRead(file);
                return CSharpSyntaxTree.ParseText(SourceText.From(stream), path: file);
            }

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
