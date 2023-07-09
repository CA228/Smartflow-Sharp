using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Bussiness.Models
{
    public class StatisticsInstance
    {
        public virtual string Code
        {
            get;
            set;
        }

        public virtual string ParentCode
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Url
        {
            get;
            set;
        }

        public virtual int Total
        {
            get;
            set;
        }
    }
}
