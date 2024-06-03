namespace HalloDocDAL.Model
{
    public class ChatModel
    {
        public int Requestid { get; set; }

        public string chatWith { get; set; }

        public bool isUser { get; set; }

        public int SenderId { get; set; }

        public string SenderType { get; set; }

        public int ReceiverId { get; set; }

        public string ReceiverType { get; set; }

        public int Adminid { get; set; }

        public string AdminName { get; set; }

        public int Physicianid { get; set; }

        public string PhysicianName { get; set; }

        public int Patientid { get; set; }

        public string Patientname { get; set; }
    }
}
