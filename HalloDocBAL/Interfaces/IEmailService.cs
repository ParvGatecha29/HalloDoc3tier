namespace HalloDocBAL.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
        void SendEmailWithAttachment(string to, string subject, string body, List<string> filesToSend);
    }
}
