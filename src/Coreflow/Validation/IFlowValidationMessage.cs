using Coreflow.Runtime;

namespace Coreflow.Validation.Messages
{
    public interface IFlowValidationMessage : IIdentifiable
    {
        bool IsFatalError { get; }

        string Message { get; }
    }
}
