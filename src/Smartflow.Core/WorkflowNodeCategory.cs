using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Core
{
    /// <summary>
    /// 工作流节点类型
    /// </summary>
    public enum WorkflowNodeCategory
    {
        /// <summary>
        /// 开始节点
        /// </summary>
        Start,

        /// <summary>
        /// 普通节点
        /// </summary>
        Node,

        /// <summary>
        /// 决策节点
        /// </summary>
        Decision,

        /// <summary>
        /// 结束节点
        /// </summary>
        End,

        /// <summary>
        /// 分叉网关
        /// </summary>
        Fork,

        /// <summary>
        /// 聚合网关
        /// </summary>
        Join
    }
}
