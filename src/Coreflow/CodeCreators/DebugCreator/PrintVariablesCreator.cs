using System;
using System.Linq;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Coreflow.CodeCreators
{
    public class PrintVariablesCreator : ICodeCreator, IUiDesignable
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string FactoryIdentifier { get; set; }

        public string Name => "Print Variables";

        public string Icon => "fa-bug";

        public string Category => "Debug";

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
            pBuilderContext.UpdateCurrentSymbols();

            pCodeWriter.WriteIdentifierTagTop(this);

            // pBuilderContext.CurrentSymbols

            var symbols = pBuilderContext.CurrentSymbols
                .Where(s => s.Kind == SymbolKind.Property || s.Kind == SymbolKind.Local || s.Kind == SymbolKind.Field);
            
            string message = "Current Variables: \" + global::System.Environment.NewLine + \"";

            foreach (var symbol in symbols)
            {
                message += $"    {symbol.Name}: \" + {symbol.Name} + global::System.Environment.NewLine + \"";

            }

            pCodeWriter.AppendLoggingCode(LogLevel.Debug, message);
        }
    }
}
