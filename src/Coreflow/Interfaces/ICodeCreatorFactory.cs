using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Interfaces
{
    public interface ICodeCreatorFactory
    {
        string Identifier { get; }

        ICodeCreator Create();
    }
}
