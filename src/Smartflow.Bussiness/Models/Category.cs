using System;

namespace Smartflow.Bussiness.Models
{
    public class Category
    {
        public virtual int Id
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

        public virtual int Sort
        {
            get;
            set;
        }
    }
}
