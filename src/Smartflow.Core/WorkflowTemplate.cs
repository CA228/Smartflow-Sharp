using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Core
{
    public class WorkflowTemplate
    {
        public virtual long Id
        {
            get;
            set;
        }

        public virtual int Status
        {
            get;
            set;
        }
        

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Source
        {
            get;
            set;
        }

        public virtual decimal Version
        {
            get;
            set;
        }

        public virtual DateTime CreateTime
        {
            get;
            set;
        }

        public virtual DateTime UpdateTime
        {
            get;
            set;
        }

        public virtual string CategoryName
        {
            get;
            set;
        }

        public virtual string CategoryCode
        {
            get;
            set;
        }

        public virtual string Memo
        {
            get;
            set;
        }
    }
}
