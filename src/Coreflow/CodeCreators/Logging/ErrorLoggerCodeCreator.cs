namespace Coreflow.CodeCreators.Logging
{
    public class ErrorLoggerCodeCreator : AbstractLoggingCodeCreator
    {
        protected override string LogLevel => "Error";
    }
}
