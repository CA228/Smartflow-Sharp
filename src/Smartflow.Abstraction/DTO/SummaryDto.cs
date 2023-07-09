using System;

namespace Smartflow.Abstraction.DTO
{
    public class SummaryDto
    {
        public virtual string CategoryName
        {
            get;
            set;
        }
        public virtual string InstanceID
        {
            get;
            set;
        }
        public virtual string CategoryCode
        {
            get;
            set;

        }
        public virtual string Comment
        {
            get;
            set;
        }
        public virtual string Key
        {
            get;
            set;
        }
        public virtual string Creator
        {
            get;
            set;

        }
        public virtual DateTime CreateTime
        {
            get;
            set;
        }

        public virtual string NodeName
        {
            get;
            set;
        }

        public virtual string NodeID
        {
            get;set;
        }

        public virtual string State
        {
            get;
            set;
        }

        public virtual string StateName
        {
            get
            {
                string result = this.State.ToLower();
                if (result == "running")
                {
                    return "流程运行中";
                }
                else if (result == "start")
                {
                    return "流程开始";
                }
                else if (result == "kill" || result == "reject")
                {
                    return "流程终止";
                }
                else if (result == "end")
                {
                    return "流程结束";
                }
                return result;
            }
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string OrganizationName
        {
            get;
            set;
        }
    }
}