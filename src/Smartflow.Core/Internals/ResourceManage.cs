using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Smartflow.Core.Internals
{
    internal partial class ResourceManage
    {
        /// <summary>
        /// 验证MAIL地址，正则表达式
        /// </summary>
        public const string MAIL_URL_EXPRESSION = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";

        public const string NOTIFICATION_RECORD_TITLE = "消息通知";
        
        public const string NOTIFICATION_TASK_TITLE = "任务待办通知";

        public const string NOTIFICATION_TASK_CONTENT_DEFAULT = "你有一条新的任务待办";

        public const string NOTIFICATION_TASK_CONTENT = "{0}/你有一条新的任务待办";
    }
}
