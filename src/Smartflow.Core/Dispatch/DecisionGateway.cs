using Smartflow.Core.Elements;
using System;
using System.Linq;

namespace Smartflow.Core.Dispatch
{
    public class DecisionGateway : DispatchAbstractCore, IGateway
    {
        public DecisionGateway(WorkflowInstance instance) : base(instance)
        {
        }

        public void Accept(IDispatchVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
