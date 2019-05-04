using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Interfaces
{
    public interface IUiDesignable
    {
        string Name { get; }

        string Icon { get; }

        string Category { get; }
    }
}
