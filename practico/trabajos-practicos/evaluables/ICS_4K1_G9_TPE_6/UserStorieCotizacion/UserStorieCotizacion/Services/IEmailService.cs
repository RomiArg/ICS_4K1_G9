using UserStorieCotizacion.Models;

namespace UserStorieCotizacion.Services
{
    public interface IEmailService
    {

        void SendEmail(EmailModel emailModel);
        void SendWelcomeEmail(string emailModel, string name, long cotizacionId);
    }
}
