using System;
using System.Collections.Generic;

namespace Smartflow.Core
{
    /// <summary>
    /// 启动参数
    /// </summary>
    public class WorkflowStartInput
    {
        public string BusinessId { get; set; }
        public string Creator { get; set; }
        public string CategoryCode { get; set; }
        public IList<string> Users { get; set; }
        public IList<string> Roles { get; set; }
        public string Message { get; set; }
        public string Props { get; set; }

        public bool Parallel { get; set; } = false;

        public IList<WorkflowSubprocess> Children { get; set; } = new List<WorkflowSubprocess>();

    }

    /// <summary>
    /// 提交参数
    /// </summary>
    public class WorkflowSubmitInput
    {
        /// <summary>
        /// 实例Id
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// 任务Id
        /// </summary>
        public long TaskId
        {
            get;
            set;
        }

        /// <summary>
        /// 0：默认 1：关闭当前任务，不做流转
        /// </summary>
        public int Close { get; set; } = 0;
        
        /// <summary>
        /// 子任务是否并行
        /// </summary>
        public bool Parallel { get; set; } = false;

        /// <summary>
        /// 线Id
        /// </summary>
        public string LineId
        {
            get;
            set;
        }

        /// <summary>
        /// 参与者
        /// </summary>
        public IList<string> Users
        {
            get;
            set;
        }

        /// <summary>
        /// 参与角色
        /// </summary>
        public IList<string> Roles
        {
            get;
            set;
        }

        /// <summary>
        /// 属性
        /// </summary>
        public string Props { get; set; }

        /// <summary>
        /// 提交者
        /// </summary>
        public string Submiter
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 子流程
        /// </summary>
        public IList<WorkflowSubprocess> Children { get; set; } = new List<WorkflowSubprocess>();
    }

    public class WorkflowSubprocess
    {
        public string InstanceId
        {
            get;
            set;
        }

        public long TemplateId { get; set; }

        public string Code { get; set; }

        public IList<string> Users { get; set; }

        public IList<string> Roles { get; set; }

        public string Props { get; set; }
    }

    public class WorkflowStartTask
    {
        public string Code { get; set; }

        public string InstanceId { get; set; }

        public long TemplateId { get; set; }
    }
}
