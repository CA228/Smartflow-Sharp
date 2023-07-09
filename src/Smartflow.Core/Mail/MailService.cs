using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Smartflow.Core.Internals;

namespace Smartflow.Core.Mail
{
    public class MailService : IMailService
    {
        private static readonly Lazy<MailConfiguration> configuration = new Lazy<MailConfiguration>(() => (MailConfiguration.Configure()));
        private static readonly Lazy<SmtpClient> smtpClient = new Lazy<SmtpClient>(() => new SmtpClient());

        static MailService()
        {
            MailConfiguration mailConfiguration = configuration.Value;
            if (mailConfiguration != null)
            {
                SmtpClient _smtp = smtpClient.Value;
                _smtp.Host = mailConfiguration.Host;
                _smtp.Port = mailConfiguration.Port;
                _smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                _smtp.EnableSsl = mailConfiguration.EnableSsl == 1;
                _smtp.UseDefaultCredentials = true;
                _smtp.Credentials = new NetworkCredential(mailConfiguration.Account, mailConfiguration.Password);
            }
        }

        public void Notification(string title, IList<string> to, string body)
        {
            if (to != null && to.Count > 0)
            {
                MailConfiguration mailConfiguration = configuration.Value;
                List<MailMessage> msgList = GetSendMessageList(mailConfiguration.Account, mailConfiguration.Name, to, title, body);
                foreach (var message in msgList)
                {
                    smtpClient.Value.Send(message);
                }
            }
        }

        protected List<MailMessage> GetSendMessageList(string from, string sender, IList<string> receiveArray, string subject, string body)
        {
            if (receiveArray.Any(MAddress => !Regex.IsMatch(MAddress, ResourceManage.MAIL_URL_EXPRESSION)))
                return null;
            List<MailMessage> messageList = new List<MailMessage>();

            foreach (string receive in receiveArray)
            {
                MailMessage message = new MailMessage(new MailAddress(from, sender), new MailAddress(receive))
                {
                    Subject = subject,
                    SubjectEncoding = Encoding.UTF8,
                    Body = body,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true,
                    Priority = MailPriority.Normal
                };
                messageList.Add(message);
            }
            return messageList;
        }
    }
}
