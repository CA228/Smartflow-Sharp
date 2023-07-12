using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Core.Dispatch
{
    public interface IGateway
    {
        void Accept(IDispatchVisitor visitor);
    }
}
