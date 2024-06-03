using HalloDocBAL.Interfaces;
using HalloDocDAL.Contacts;
using HalloDocDAL.Model;
using System.Net;
using System.Net.Mail;

namespace HalloDocBAL.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly IRequestRepository _requestRepository;

        public EmailService(SmtpSettings smtpSettings, IRequestRepository requestRepository)
        {
            _smtpSettings = smtpSettings;
            _requestRepository = requestRepository;
        }

        public void SendEmail(string to, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
            {
                client.EnableSsl = _smtpSettings.EnableSSL;
                client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(to);
                client.Send(mailMessage);
                _requestRepository.EmailLog(to, subject, body);
            }
        }

        public void SendEmailWithAttachment(string to, string subject, string body, List<string> filesToSend)
        {
            using (var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
            {
                client.EnableSsl = _smtpSettings.EnableSSL;
                client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(to);
                foreach (var file in filesToSend)
                {
                    var attachment = new Attachment(file);
                    mailMessage.Attachments.Add(attachment);
                }
                client.Send(mailMessage);
                _requestRepository.EmailLog(to, subject, body);
            }
        }
    }
}
