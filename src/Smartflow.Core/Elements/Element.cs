using System;

namespace Smartflow.Core.Elements
{
    public abstract class Element 
    {
        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Id
        {
            get;
            set;
        }
    }
}
