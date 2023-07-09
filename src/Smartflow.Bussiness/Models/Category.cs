using System;

namespace Smartflow.Bussiness.Models
{
    public class Category
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
    }
}
