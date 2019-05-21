namespace Coreflow.Validation
{
    public interface ICorrector
    {
        string Name { get; }

        bool CanCorrect();

        object GetData();

        string GetSerializedData();
    }
}