using BusinessObjects.ConfigurationModels;

namespace Hosteland.Services.EmailService {
    public interface IEmailService {
       void SendEmail(Message message);
    }
}
