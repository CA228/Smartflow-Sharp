using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Core.Dispatch
{
    public sealed class DispatchTaskStrategy
    {
        private readonly IDispatch dispatchTask;

        public static DispatchTaskStrategy CreatetDispatchTaskStrategy(IDispatch dispatchTask)
        {
            return new DispatchTaskStrategy(dispatchTask);
        }

        private DispatchTaskStrategy(IDispatch dispatchTask)
        {
            this.dispatchTask = dispatchTask;
        }

        public void Dispatch()
        {
            dispatchTask.Dispatch();
        }
    }
}
