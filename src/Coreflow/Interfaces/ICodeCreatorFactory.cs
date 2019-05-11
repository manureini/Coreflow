namespace Coreflow.Interfaces
{
    public interface ICodeCreatorFactory
    {
        string Identifier { get; }

        ICodeCreator Create();
    }
}
