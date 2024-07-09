namespace BusinessObjects.DTOs {
    public class SendEmailRequestDto {
        public string Content { get; set; }
        public string Subject { get; set; }
        public string[] Emails { get; set; }
    }
}
