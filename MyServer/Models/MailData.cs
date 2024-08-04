namespace MyServer.Models
{
    public class MailData
    {
        //Receiver
        public List<string> To{ get; }

        //Content
        public string Subject { get; }
        public string? Body { get; }

        public MailData(
            List<string> to,
            string subject,
            string? body = null)
        {
            //Receiver
            To = to;


            //Content
            Subject = subject;
            Body = body;
        }
    }   
}
