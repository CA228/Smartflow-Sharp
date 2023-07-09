using System;
using System.Collections.Generic;

namespace Smartflow.Core.Elements
{
    public abstract class ASTNode : Element
    {
        protected ISet<Transition> transitions = new HashSet<Transition>();
        protected WorkflowNodeCategory category = WorkflowNodeCategory.Node;

        public virtual ISet<Transition> Transitions
        {
            get { return transitions; }
            set { transitions = value; }
        }

        public virtual WorkflowNodeCategory NodeType
        {
            get { return category; }
            set { category = value; }
        }
    }
}