using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Linq;

namespace Coreflow.Activities.Common
{
    public class SendEmail : ICodeActivity
    {

        public void Execute(
            string pHost,
            int pPort,
            bool pEnableSsl,
            string pUsername,
            string pPassword,
            string pSubject,
            string pBody,
            IEnumerable<Attachment> pAttachments,
            MailAddress pFrom,
            IEnumerable<MailAddress> pTo,
            IEnumerable<MailAddress> pCc,
            IEnumerable<MailAddress> pBcc
            )
        {
            SmtpClient client = new SmtpClient
            {
                Host = pHost,
                Port = pPort,
                EnableSsl = pEnableSsl,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(pUsername, pPassword),
            };

            MailMessage mm = new MailMessage
            {
                From = pFrom,
                Subject = pSubject,
                Body = pBody,
                BodyEncoding = UTF8Encoding.UTF8,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                IsBodyHtml = true,
                Priority = MailPriority.Normal,
                SubjectEncoding = UTF8Encoding.UTF8,
            };


            pAttachments?.ForEach(a => mm.Attachments.Add(a));

            pTo.ForEach(e => mm.To.Add(e));
            pCc?.ForEach(e => mm.CC.Add(e));
            pBcc?.ForEach(e => mm.Bcc.Add(e));

            mm.Headers.Add("X-Mailer", "SMTP Client");
            mm.Headers.Add("Message-Id", $"<{Guid.NewGuid()}@smtpclient>");

            client.Send(mm);
        }
    }
}