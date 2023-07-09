using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Core
{
    /// <summary>
    /// 流程实例状态
    /// </summary>
    public enum WorkflowInstanceState
    {
        /// <summary>
        /// 运行中
        /// </summary>
        Running,

        /// <summary>
        /// 挂起
        /// </summary>
        Hang,

        /// <summary>
        /// 流程完结
        /// </summary>
        End,

        /// <summary>
        /// 流程驳回
        /// </summary>
        Reject,

        /// <summary>
        /// 流程终止
        /// </summary>
        Termination,

        /// <summary>
        /// 杀死流程
        /// </summary>
        Kill
    }
}
