using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Core.Dispatch
{
    public interface IDispatchVisitor
    {
        void Visit(DecisionGateway decision);
        
        void Visit(ForkGateway fork);

        void Visit(JoinGateway join);
    }
}
