using Coreflow.Interfaces;
using Coreflow.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Coreflow
{
    public abstract class AbstractLoggingCodeCreator : ICodeCreator, IParametrized, IUiDesignable
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string FactoryIdentifier { get; set; }

        public string Name { get => "Log " + LogLevel; }

        public string Icon => "fa-edit";

        public string Category => "Logging";

        public List<IArgument> Arguments { get; set; }

        protected abstract string LogLevel { get; }

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer = null)
        {
            pCodeWriter.WriteIdentifierTagTop(this);

            pCodeWriter.AppendLineTop($"Microsoft.Extensions.Logging.LoggerExtensions.Log{LogLevel}({nameof(ICompiledFlow.Logger)}, ");
            Arguments[0].ToCode(pBuilderContext, pCodeWriter, pContainer);
            pCodeWriter.AppendTop(",");
            Arguments[1].ToCode(pBuilderContext, pCodeWriter, pContainer);
            pCodeWriter.AppendTop(",");
            Arguments[2].ToCode(pBuilderContext, pCodeWriter, pContainer);
            pCodeWriter.AppendTop(",");
            Arguments[3].ToCode(pBuilderContext, pCodeWriter, pContainer);
            pCodeWriter.AppendTop(");");
        }

        //    void LogInformation(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args);

        public CodeCreatorParameter[] GetParameters()
        {
            return new[] {
                new CodeCreatorParameter() {
                 Direction = VariableDirection.In,
                 Name = "EventId",
                 Type = typeof(EventId)
                },
                new CodeCreatorParameter() {
                 Direction = VariableDirection.In,
                 Name = "Exception",
                 Type = typeof(Exception)
                },
                new CodeCreatorParameter() {
                 Direction = VariableDirection.In,
                 Name = "Message",
                 Type = typeof(string)
                },
                new CodeCreatorParameter() {
                 Direction = VariableDirection.In,
                 Name = "Args",
                 Type = typeof(IEnumerable<object>)
                }
            };
        }

    }
}
