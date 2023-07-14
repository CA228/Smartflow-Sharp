﻿using System;

namespace Smartflow.Core.Elements
{
    public class Transition : Element
    {
        private string destination = string.Empty;
        private string expression = string.Empty;
        private string order = string.Empty;

        public virtual string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public virtual string Order
        {
            get { return order; }
            set { order = value; }
        }

        public virtual string Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        public virtual string Url
        {
            get;
            set;
        }
    }
}
