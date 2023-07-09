using System;
using System.Linq;
using NHibernate;
using Smartflow.Common;

namespace Smartflow.Core.Mail
{
    public  class MailConfiguration
    {
        public virtual int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 账户名
        /// </summary>
        public virtual string Account
        {
            get;
            set;
        }

        /// <summary>
        /// 密码（授权码）
        /// </summary>
        public virtual string Password
        {
            get;
            set;
        }

        /// <summary>
        /// 发送邮件显示的名称
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 服务器smtp.163.com
        /// </summary>
        public virtual string Host
        {
            get;
            set;
        }

        /// <summary>
        /// 端口(25)
        /// </summary>
        public virtual int Port
        {
            get;
            set;
        }

        /// <summary>
        /// 启用HTTPS
        /// </summary>
        public virtual int EnableSsl
        {
            get;
            set;
        }

        public static MailConfiguration Configure()
        {
            using ISession session = DbFactory.OpenSession();
            return session.Query<MailConfiguration>().FirstOrDefault();
        }
    }
}
