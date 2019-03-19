using System.Collections.Generic;

namespace Coreflow.Interfaces
{
    public interface ICodeCreatorContainerCreator : ICodeCreator, IIdentifiable
    {
        ICodeCreatorContainerCreator ParentContainerCreator { get; set; }

        List<List<ICodeCreator>> CodeCreators { get; set; }
    }
}
