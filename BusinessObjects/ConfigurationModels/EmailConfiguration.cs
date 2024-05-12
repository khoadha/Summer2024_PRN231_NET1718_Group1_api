using MimeKit;
using System.Text;

namespace BusinessObjects.ConfigurationModels {
    public class EmailConfiguration {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class Message {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message(IEnumerable<string> to, string subject, string content) {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(Encoding.UTF8, "", x)));
            Subject = subject;
            Content = content;
        }
    }
}
