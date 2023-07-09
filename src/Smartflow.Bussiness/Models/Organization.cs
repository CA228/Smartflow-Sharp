using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Bussiness.Models
{
    public class Organization
    {
        public virtual string Id
        {
            get;
            set;
        }

        public virtual string ParentId
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }
    }
}
