namespace Coreflow.Interfaces
{
    public interface IArgument : ICodeCreator
    {
        //This Name should be the name of the CodeCreatorParameter
        //But if the method is changed like new parameter order or new parameter etc the Argument can maybe be mapped to the new "slot"
        string Name { get; set; }

        string Code { get; set; }
    }
}
