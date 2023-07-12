using System;
using Smartflow.Core.Elements;

namespace Smartflow.Core.Dispatch
{
    public class ForkGateway : DispatchAbstractCore, IGateway
    {
        public ForkGateway(WorkflowInstance instance) : base(instance)
        {

        }

        public void Accept(IDispatchVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
