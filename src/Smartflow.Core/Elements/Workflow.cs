using System;
using System.Collections.Generic;

namespace Smartflow.Core.Elements
{
    public class Workflow
    {
        private List<Node> nodes = new List<Node>();

        /// <summary>
        /// 流程节点
        /// </summary>
        public virtual List<Node> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }
    }
}
