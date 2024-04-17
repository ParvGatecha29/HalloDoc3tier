namespace HalloDocDAL.Model
{
    public class EmailAttachment
    {
        public EmailAttachment(byte[] content, string fileName, string contentType)
        {
            Content = content;
            FileName = fileName;
            ContentType = contentType;
        }

        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
