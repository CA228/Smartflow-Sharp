using System;
using Smartflow.Core.Elements;
using System.Linq;
using System.Collections.Generic;

namespace Smartflow.Core.Dispatch
{
    public class JoinGateway : DispatchAbstractCore, IGateway
    {
        public JoinGateway(WorkflowInstance instance) : base(instance)
        {
        }

        public void Accept(IDispatchVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
