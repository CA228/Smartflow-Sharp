using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Bussiness.Models
{
    public class User
    {
        public virtual string Id
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Mail
        {
            get;
            set;
        }

        public virtual string OrganizationCode
        {
            get;
            set;
        }

        public virtual string OrganizationName
        {
            get;
            set;
        }
    }
}
