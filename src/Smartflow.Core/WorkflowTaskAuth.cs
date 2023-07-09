using System;
using System.Collections.Generic;
using Smartflow.Core.Elements;

namespace Smartflow.Core
{
    public class WorkflowTaskAuth
    {
        public virtual long Id
        {
            get;
            set;
        }

        public virtual long TaskId
        {
            get;
            set;
        }

        public virtual string AuthCode
        {
            get;
            set;
        }

        public virtual int Type
        {
            get;
            set;
        }
    }
}
