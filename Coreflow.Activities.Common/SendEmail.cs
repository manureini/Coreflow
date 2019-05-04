using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Linq;
using Coreflow.Objects;

namespace Coreflow.Activities.Common
{
    [DisplayMeta("Send Mail", "Common", "fa-envelope")]
    public class SendEmail : ICodeActivity
    {
        public void Execute(
            string Host,
            int Port,
            bool EnableSsl,
            string Username,
            string Password,
            string Subject,
            string Body,
            IEnumerable<Attachment> Attachments,
            MailAddress From,
            IEnumerable<MailAddress> To,
            IEnumerable<MailAddress> Cc,
            IEnumerable<MailAddress> Bcc
            )
        {
            SmtpClient client = new SmtpClient
            {
                Host = Host,
                Port = Port,
                EnableSsl = EnableSsl,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(Username, Password),
            };

            MailMessage mm = new MailMessage
            {
                From = From,
                Subject = Subject,
                Body = Body,
                BodyEncoding = UTF8Encoding.UTF8,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                IsBodyHtml = true,
                Priority = MailPriority.Normal,
                SubjectEncoding = UTF8Encoding.UTF8,
            };


            Attachments?.ForEach(a => mm.Attachments.Add(a));

            To.ForEach(e => mm.To.Add(e));
            Cc?.ForEach(e => mm.CC.Add(e));
            Bcc?.ForEach(e => mm.Bcc.Add(e));

            mm.Headers.Add("X-Mailer", "SMTP Client");
            mm.Headers.Add("Message-Id", $"<{Guid.NewGuid()}@smtpclient>");

            client.Send(mm);
        }
    }
}