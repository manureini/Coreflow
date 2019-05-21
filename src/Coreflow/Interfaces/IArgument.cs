namespace Coreflow.Interfaces
{
    public interface IArgument : ICodeCreator
    {
        string Name { get; set; }

        string Code { get; set; }
    }
}
