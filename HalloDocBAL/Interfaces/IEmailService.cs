using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocBAL.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
        void SendEmailWithAttachment(string to, string subject, string body, List<string> filesToSend);
    }
}
