using System;
using System.Data;
using System.Collections.Generic;

namespace Smartflow.Core.Elements
{
    public class Node : ASTNode
    {
        protected ISet<Actor> actors = new HashSet<Actor>();
        protected ISet<Group> groups = new HashSet<Group>();
        protected ISet<Organization> organizations = new HashSet<Organization>();

        public virtual int Collaboration
        {
            get;
            set;
        }

        public virtual ISet<Group> Groups
        {
            get => groups;
            set { groups = value; }
        }

        public virtual ISet<Actor> Actors
        {
            get => actors;
            set { actors = value; }
        }

        public virtual ISet<Elements.Organization> Organizations
        {
            get => organizations;
            set { organizations = value; }
        }
    }
}
