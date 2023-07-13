using Smartflow.Core.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Core.Event
{
    public class EndEventArgs : EventArgs
    {
        public Element El
        {
            get;
            set;
        }

        public EndEventArgs(Element el)
        {
            this.El = el;
        }
    }
}
