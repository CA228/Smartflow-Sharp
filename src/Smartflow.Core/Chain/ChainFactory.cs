using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Core.Chain
{
    internal sealed class ChainFactory
    {
        public static ChainHandler Chain()
        {
            return new ChainHandler();
        }
    }
}
