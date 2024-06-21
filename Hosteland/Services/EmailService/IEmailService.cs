using BusinessObjects.ConfigurationModels;

namespace HostelandAuthorization.Services.EmailService {
    public interface IEmailService {
       void SendEmail(Message message);
    }
}
