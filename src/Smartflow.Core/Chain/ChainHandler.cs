using System;
using System.Collections.Generic;

namespace Smartflow.Core.Chain
{
    public class ChainHandler
    {
        private readonly Queue<IHandler> handlers= new Queue<IHandler>();

        public ChainHandler Add(IHandler handle)
        {
            handlers.Enqueue(handle);
            return this;
        }

        public void Done()
        {
            foreach (IHandler item in handlers)
            {
                item.Done();
            }
        }
    }
}
