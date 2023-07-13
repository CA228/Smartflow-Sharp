using Smartflow.Core.Elements;
using System;

namespace Smartflow.Core.Event
{
    public class StartEventArgs : EventArgs
    {
        public Element El
        {
            get;
            set;
        }

        public StartEventArgs(Element el)
        {
            this.El = el;
        }
    }
}
