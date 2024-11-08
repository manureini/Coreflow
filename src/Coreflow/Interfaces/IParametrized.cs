﻿using Coreflow.Objects;
using Coreflow.Runtime;
using System.Collections.Generic;

namespace Coreflow.Interfaces
{
    public interface IParametrized : IIdentifiable
    {
        List<IArgument> Arguments { get; set; }

        CodeCreatorParameter[] GetParameters();
    }
}
