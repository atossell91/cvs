using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvs
{
    internal class OutlookEmailer
    {
        public void SendEmail(string address, string data)
        {
            var app = new Microsoft.Office.Interop.Outlook.Application();
            foreach (var account in app.Session.Accounts)
            {
                account.S
            }
        }
    }
}
