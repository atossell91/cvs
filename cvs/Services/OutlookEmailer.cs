using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;

namespace cvs
{
    internal class OutlookEmailer
    {
        public static void SendEmail(string toAddress, string fromAddress, string subjectText, string bodyText, params string[] attachmentPaths)
        {
            var app = new Microsoft.Office.Interop.Outlook.Application();
            var email = app.CreateItem(OlItemType.olMailItem);

            email.Subject = subjectText;
            email.To = toAddress;
            email.Body = bodyText;

            foreach (var attachmentPath in attachmentPaths)
            {
                email.Attachments.Add(attachmentPath);
            }

            foreach (Microsoft.Office.Interop.Outlook.Account account in app.Session.Accounts)
            {
                if (account.SmtpAddress == fromAddress)
                {
                    email.SendUsingAccount = account;
                    email.Send();
                    break;
                }
            }
        }
    }
}
