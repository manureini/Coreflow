using System;

namespace Coreflow.Interfaces
{
    public interface ICodeCreatorFactory
    {
        string Identifier { get; }
        Type[] OverrideableCustomTypes { get; }

        ICodeCreator Create(Type[] pCustomTypes = null);
    }
}
